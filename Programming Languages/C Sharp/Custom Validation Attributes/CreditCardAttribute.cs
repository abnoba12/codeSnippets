using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace StonebridgeJCP.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class CreditCardAttribute : ValidationAttribute
    {
        //Get a list of the accepted card types for this validation
        public string cardTypeField;
        public string acceptedTypes;
        private MvcApplication MvcApplication = new MvcApplication();

        public CreditCardAttribute(string cardType, string acceptedTypes)            
        {

            this.cardTypeField = cardType;
            this.acceptedTypes = acceptedTypes;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // get a reference to the property this validation depends upon
            var containerType = validationContext.ObjectInstance.GetType();
            var cardTypefield = containerType.GetProperty(this.cardTypeField);

            if (cardTypefield != null)
            {
                // get the value of the cardType property
                var cardTypeValue = cardTypefield.GetValue(validationContext.ObjectInstance, null);
                CardType cardType = CardType.Unknown;

                // trim spaces off cardType value
                if (cardTypeValue != null && cardTypeValue is string)
                {
                    string cardTypeFieldValue = (string)cardTypeValue;
                    cardType = (CardType)Enum.Parse(typeof(CardType), cardTypeFieldValue, true);
                }

                if (value != null)
                {
                    string cardNum = (string)value;

                    if (!IsValidType(cardNum, cardType) || !IsValidCheckDigit(cardNum) || !IsAcceptedType(cardType, acceptedTypes))
                    {
                        // validation failed - return an error
                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), new[] { validationContext.MemberName });
                    }
                }
            }
            return ValidationResult.Success;
        }

        public virtual IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "creditcard",
            };

            yield return rule;
        }

        public enum CardType
        {
            Unknown = 01,
            Visa = 04,
            MasterCard = 05,
            Amex = 22,
            Diners = 16,
            Discover = 17,
            jpcReward = 06,
            All = CardType.Visa | CardType.MasterCard | CardType.Amex | CardType.Diners | CardType.Discover | CardType.jpcReward,
            AllOrUnknown = CardType.Unknown | CardType.Visa | CardType.MasterCard | CardType.Amex | CardType.Diners | CardType.Discover | CardType.jpcReward
        }

        private bool IsValidType(string cardNumber, CardType cardType)
        {
            // jcp //test card number 0123456782
            if (cardType.Equals(CardType.jpcReward))
            {
                return cardNumber.Length >= 10 && cardNumber.Length <= 11;
            }

            // Visa
            if (Regex.IsMatch(cardNumber, "^(4)") && cardType.Equals(CardType.Visa))
            {
                return cardNumber.Length == 13 || cardNumber.Length == 16;
            }

            // MasterCard
            if (Regex.IsMatch(cardNumber, "^(51|52|53|54|55)") && cardType.Equals(CardType.MasterCard))
            {
                return cardNumber.Length == 16;
            }

            // Amex
            if (Regex.IsMatch(cardNumber, "^(34|37)") && cardType.Equals(CardType.Amex))
            {
                return cardNumber.Length == 15;
            }

            // Diners
            if (Regex.IsMatch(cardNumber, "^(300|301|302|303|304|305|36|38)") && cardType.Equals(CardType.Diners))
            {
                return cardNumber.Length == 14;
            }

            // Discover
            if (Regex.IsMatch(cardNumber, "^(6011)") && cardType.Equals(CardType.Discover))
            {
                return cardNumber.Length == 16;
            }

            //Unknown
            if (cardType.Equals(CardType.Unknown))
            {
                return true;
            }

            return false;
        }

        private bool IsValidCheckDigit(string acctNbr)
        {
            //perform check digit logic for the credit card
            int[] intTwosComp = new int[21];
            int intNdx = 0;
            int intLoop = 0;
            int intCheckSumTotal = 0;
            int intOrigCheckSum = 0;
            int intTmpChkNbr = 0;

            //load our two's compliment array
            for (intNdx = 1; intNdx <= 20; intNdx++)
            {
                if ((intNdx % 2) == 0)
                {
                    intTwosComp[intNdx] = 2;
                }
                else
                {
                    intTwosComp[intNdx] = 1;
                }
            }

            intOrigCheckSum = Convert.ToInt32(acctNbr.Substring(acctNbr.Length - 1, 1));

            intNdx = 20 - acctNbr.Length;

            for (intLoop = 1; intLoop <= acctNbr.Length - 1; intLoop++)
            {
                intTmpChkNbr = Convert.ToInt32(acctNbr.Substring(intLoop - 1, 1)) * intTwosComp[intNdx];

                if (intTmpChkNbr <= 9)
                {
                    intCheckSumTotal += intTmpChkNbr;
                }
                else
                {
                    intTmpChkNbr = 1 + (intTmpChkNbr - 10);
                    intCheckSumTotal += intTmpChkNbr;
                }

                intNdx += 1;
            }

            int intChkSumMod10 = intCheckSumTotal % 10;
            if (intChkSumMod10 > 0)
            {
                intChkSumMod10 = 10 - intChkSumMod10;
            }

            if (intOrigCheckSum == intChkSumMod10)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsAcceptedType(CardType cardType, string acceptedTypes)
        {
            string[] acceptedTypesArray = acceptedTypes.Split(',');
            foreach (string type in acceptedTypesArray)
            {
                CardType acceptedCardEnum = (CardType)Enum.Parse(typeof(CardType), type, true);
                if (acceptedCardEnum == cardType)
                {
                    return true;
                }
            }
            return false;
        }
    }
}