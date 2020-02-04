using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EEG_ReelCinemasRESTAPI.Models.Response.GiftCard
{
    public class CompleteOrderPaymentByGiftCardResp
    {
        public object BackgroundJobUrl { get; set; }
        public object BalanceList { get; set; }
        public string CinemaID { get; set; }
        public object ErrorDescription { get; set; }
        public int ExtendedResultCode { get; set; }
        public string HistoryID { get; set; }
        public object LoyaltyPointsCost { get; set; }
        public object PassCollection { get; set; }
        public Paymentinfocollection[] PaymentInfoCollection { get; set; }
        public string PrintStream { get; set; }
        public object PrintStreamCollection { get; set; }
        public int Result { get; set; }
        public string VistaBookingId { get; set; }
        public string VistaBookingNumber { get; set; }
        public string VistaTransNumber { get; set; }

        public string MobileBookingId { get; set; }
    }

    public class Paymentinfocollection
    {
        public object BankReference { get; set; }
        public object BankTransactionNumber { get; set; }
        public bool BillFullOutstandingAmount { get; set; }
        public int BillingValueCents { get; set; }
        public int CardBalance { get; set; }
        public string CardCVC { get; set; }
        public string CardExpiryMonth { get; set; }
        public string CardExpiryYear { get; set; }
        public string CardHash { get; set; }
        public string CardIssueNumber { get; set; }
        public string CardNumber { get; set; }
        public string CardType { get; set; }
        public string CardValidFromMonth { get; set; }
        public string CardValidFromYear { get; set; }
        public object CustomerTaxName { get; set; }
        public object CustomerTaxNumber { get; set; }
        public object MemberId { get; set; }
        public string PaymentErrorCode { get; set; }
        public string PaymentErrorDescription { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentSystemId { get; set; }
        public string PaymentTenderCategory { get; set; }
        public object PaymentToken { get; set; }
        public object PaymentTokenType { get; set; }
        public int PaymentValueCents { get; set; }
        public bool SaveCardToWallet { get; set; }
        public bool UseAsBookingRef { get; set; }
        public object WalletAccessToken { get; set; }
    }

}