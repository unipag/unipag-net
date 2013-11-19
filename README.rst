Unipag Client for .Net
======================

Requirements
------------

* .NET Framework versions from 2.0 to 4.5
* Newtonsoft.Json versions 4.5.x or 5.0.x

Installation
------------

Install using NuGet (recommended)::

    PM> Install-Package Unipag

API keys configuration
----------------------

To work with Unipag API, you will need to provide an API key. Get your
API keys at `<https://my.unipag.com>`_ > API keys. API key can be defined
globally in your App.config as following:

.. code:: xml

    <appSettings>
        <add key="UnipagApiKey" value="<your-secret-API-key>"/>
    </appSettings>

Another way of setting Unipag API key globally is to pass it to Unipag.Config
class:

.. code:: c#

    Unipag.Config.ApiKey = "<your-secret-API-key>";

Please note that Unipag.Config setting will have priority over App.config.

API keys for multiple Unipag accounts
-------------------------------------

If your application deals with multiple Unipag accounts, you may find that the
most convenient way will be to pass API keys directly to methods which
interact with Unipag API:

.. code:: c#

    var invoice = Unipag.Invoice.Get("424242424242", "<API-key-for-this-account>");

Providing API key directly in method call has highest priority and will override
both App.config and Unipag.Config settings.

When object instance is retrieved from Unipag, API key is stored in its ApiKey
property and will be used for subsequent operations this object, so you will not
have to provide it again. Consider the following example:

.. code:: c#

    Unipag.Config.ApiKey = "<default-API-key>";

    var invoice1 = Unipag.Invoice.Get("424242424242", "<API-key-1>");

    var invoice2 = Unipag.Invoice.Get("900090009000", "<API-key-2>");

    invoice1.Customer = "John Doe";
    invoice1.Save();  // <API-key-1> will be used

    invoice2.Delete();  // <API-key-2> will be used

    var connections = Unipag.Connection.List();  // <default-API-key> will be used

Sample usage
------------

Create invoice
~~~~~~~~~~~~~~

Create a new instance of Unipag.Invoice class and either call .Save() on it,
or pass to Invoice.Create method. Both methods return Invoice instance with
latest information updated from Unipag. Here is an example with Invoice.Create:

.. code:: c#

    using System;
    using Unipag;

    namespace MyApp
    {
        class Program
        {
            static void Main(string[] args)
            {
                Config.ApiKey = "<your-secret-API-key>";

                var invoice = Invoice.Create(new Invoice
                {
                    Amount = 42,
                    Currency = "USD",
                });

                Console.WriteLine(invoice.ToString());
                // Output:
                // {
                //   "account": "acc_your-account-ID",
                //   "amount": 42,
                //   "amount_paid": 0,
                //   "created": "2013-08-20T09:00:14Z",
                //   "currency": "USD",
                //   "custom_data": null,
                //   "customer": "",
                //   "deleted": false,
                //   "description": "",
                //   "expires": null,
                //   "id": "111631341369",
                //   "modified": "2013-08-20T09:00:14Z",
                //   "object": "invoice",
                //   "reference": "",
                //   "test_mode": false
                // }


                // Now let's modify it:
                invoice.Amount = 9000;
                invoice.Save();

                Console.WriteLine(invoice.ToString());
                // Output:
                // {
                //   "account": "acc_your-account-ID",
                //   "amount": 9000,
                //   "amount_paid": 0,
                //   "created": "2013-08-20T09:00:14Z",
                //   "currency": "USD",
                //   "custom_data": null,
                //   "customer": "",
                //   "deleted": false,
                //   "description": "",
                //   "expires": null,
                //   "id": "111631341369",
                //   "modified": "2013-08-20T09:00:15Z",
                //   "object": "invoice",
                //   "reference": "",
                //   "test_mode": false
                // }

                Console.ReadLine();
            }
        }
    }



Handle webhook from Unipag
~~~~~~~~~~~~~~~~~~~~~~~~~~

Create a standalone page on your website which will handle events sent by
Unipag. Register URL of this page at `<https://my.unipag.com>`_ > Webhooks.
Initialize page code as following (example for ASP.NET MVC):

.. code:: c#

    using System.Net;
    using System.Web.Mvc;
    using Unipag;

    namespace MyApp.Controllers
    {
        public class WebhooksController : Controller
        {
            [AcceptVerbs(HttpVerbs.Post)]
            public ActionResult UnipagEvent()
            {
                Config.ApiKey = "<your-secret-API-key>";

                // Read incoming event
                var postData = new System.IO.StreamReader(Request.InputStream).ReadToEnd();
                var incomingEvent = new Event();
                incomingEvent.FromString(postData);

                // In this example we subscribe to invoice-related events only
                if (incomingEvent.RelatedObject is Invoice)
                {
                    var unipagInvoice = (Invoice)incomingEvent.RelatedObject;

                    // Reload information from Unipag for security reasons
                    unipagInvoice.Reload();

                    // ... do something with invoice data ...
                }

                // Return response with code 200 to tell Unipag that message was delivered
                return new HttpStatusCodeResult((int)HttpStatusCode.OK);
            }
        }
    }


Tip: webhooks can be a pain to debug. Check out Unipag Network Activity log, it
is available at `<https://my.unipag.com>`_ > Network Activity. You may find it
useful for your webhook handlers debugging.

Usage of Invoice.CustomData property
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Invoice objects in Unipag API have an optional "custom_data" field, which can
store up to 32KB of arbitrary data in JSON format. You can freely use this field
to store additional application-specific information about your invoices.

In Unipag Client for .Net, custom_data field is mapped into CustomData property
and has JToken type. Please refer to
`Json.Net documentation <http://james.newtonking.com/projects/json/help/>`_
for full description of JToken API. Here is some very basic example:

.. code:: c#

    using System;
    using Unipag;

    namespace MyApp
    {
        class Program
        {
            static void Main(string[] args)
            {
                Config.ApiKey = "<your-secret-API-key>";

                var inv = new Invoice
                {
                    Amount = 3,
                    Currency = "RUB",
                };
                inv.CustomData["text"] = "Some text";
                inv.CustomData["int"] = 9000;
                inv.CustomData["decimal"] = 42.5m;
                inv.Save();

                Console.WriteLine(inv.CustomData.ToString());
                // Output:
                // {
                //   "decimal": 42.5,
                //   "int": 42,
                //   "text": "Some text"
                // }

                Console.WriteLine(inv.CustomData.Value<decimal>("decimal"));
                // Output:
                // 42.5

                Console.ReadLine();
            }
        }
    }

Report bugs
-----------

Report issues to the project's `Issues Tracking`_ on Github.

.. _`Issues Tracking`: https://github.com/unipag/unipag-net/issues
