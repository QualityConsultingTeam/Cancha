using Access.Models;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesAccess
{
    public class PaymentManager
    {

        public   void PendingPayments()
        {

            //configuracion de paypal
            APIContext api = getPaypalContext();

            //var pendingPaniers = await Context.Panier.Where(x => x.Status == PanierStatus.Pending && x.paymentId != null).ToListAsync();

            //pendingPaniers.ForEach(x =>
            //{
            //    var paymentStatus = Payment.Get(api, x.paymentId);

            //});
        }


        public PayPal.Api.Payment GetPayment (Booking booking  )
        {

            // var panier = await GetActivePanier(true);

            //var titres = panier.PanierAlbums.SelectMany(x => x.Titres).Select(x => new Item { name = x.Nom, currency = "EUR", price = x.Prix.ToString(), quantity = "1" });

            //var total = panier.PanierAlbums.Sum(x => x.Total);

            var item = new Item()
            {
                name = string.Format("Reserva de Cancha {0} " , booking.Field.Name),
                currency ="USD",
                quantity ="1",
                price = booking.Price.ToString(),
            };
            //configuracion de paypal
            APIContext api = getPaypalContext();

            List<Item> items = new List<Item>();
            items.Add(item);

            ItemList itemList = new ItemList();
            itemList.items = items;



            Amount amnt = new Amount();
            amnt.currency = "EUR";
            amnt.total = booking.Price.ToString();

            List<Transaction> transactionList = new List<Transaction>();
            Transaction tran = new Transaction();
            //tran.description = "creating a payment";
            tran.amount = amnt;
            tran.item_list = itemList;

            transactionList.Add(tran);

            Payer payr = new Payer();
            payr.payment_method = "paypal";

            RedirectUrls redirUrls = new RedirectUrls();
            redirUrls.cancel_url = cancel_url;
            redirUrls.return_url = return_url;

            PayPal.Api.Payment pymnt = new PayPal.Api.Payment();
            pymnt.intent = "sale";
            pymnt.payer = payr;
            pymnt.transactions = transactionList;
            pymnt.redirect_urls = redirUrls;

            var payment = pymnt.Create(api);
            if (pymnt != null)
            {

                //panier.paymentId = payment.id;
                //await SaveAsync();
            }

            return payment;
        }


        private   APIContext getPaypalContext()
        {
            Dictionary<string, string> configurationPayment = new Dictionary<string, string>();
            configurationPayment.Add("mode", IsPayPalSandBox ? "sandbox" : "live");
            OAuthTokenCredential tokenCredential = new OAuthTokenCredential(clientId, Secret, configurationPayment);
            var api = new APIContext(tokenCredential.GetAccessToken());
            api.Config = configurationPayment;
            return api;
        }


        public bool ConfirmPayment(string paymentId, string token, string PayerID)
        {
           // var panier = await Context.Panier.FirstOrDefaultAsync(u => u.owner == UserId && u.Status == PanierStatus.Active && u.paymentId == paymentId);

            if (  !string.IsNullOrEmpty(paymentId) && !string.IsNullOrEmpty(PayerID))
            {
                //configuracion de paypal
                APIContext api = getPaypalContext();

                PayPal.Api.Payment payment = PayPal.Api.Payment.Get(api, paymentId);
                PaymentExecution pymntExecution = new PaymentExecution();
                pymntExecution.payer_id = (PayerID);
                PayPal.Api.Payment executedPayment = payment.Execute(api, pymntExecution);
                if (executedPayment != null && executedPayment.state == "approved")
                {
                    ///crear la factura
                    //var total = payment.transactions.Sum(t => Convert.ToDecimal(t.amount.total));
                    //var achat = new Achat() { Panier = panier, UtilisateurId = UserId, Prix = total };
                    //Context.Achats.Add(achat);
                    //panier.Status = PanierStatus.Confirmed;
                    //panier.PayerID = PayerID;
                    //await SaveAsync();

                }

            }

            return false;
        }



        #region SanboxConfig
        private bool IsPayPalSandBox
        {
            get
            {
                if (!_isSandbox.HasValue)
                {
                    var _sandbox = ConfigurationManager.AppSettings["IsPayPalSandbox"];
                    _isSandbox = !string.IsNullOrEmpty(_sandbox) && Convert.ToBoolean(ConfigurationManager.AppSettings["IsPayPalSandbox"]);
                }
                return _isSandbox.Value;
            }
        }


        private string clientId = ConfigurationManager.AppSettings["clientId"];

        private string Secret =  ConfigurationManager.AppSettings["Secret"];

        private string return_url = ConfigurationManager.AppSettings["return_url"];
        private string cancel_url = ConfigurationManager.AppSettings["cancel_url"];


        private bool? _isSandbox { get; set; }
        #endregion  
    }
}
