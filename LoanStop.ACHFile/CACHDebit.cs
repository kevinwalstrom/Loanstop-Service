using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoanStop.ACHFile
{
    public class CACHDebit
    {

        protected string sName;
        protected string sRoutingNumber;
        protected string sAccountNumber;

        public string SSNumber;
        public decimal Amount;
        public string RoutingCheckDigit;
        public string StoreCode;

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {

                if (sName.Length != 22)
                {
                    if (sName.Length < 22)
                    {
                        do
                        {
                            sName = sName + " ";
                        } while (sName.Length < 22);
                        return sName.Substring(0, 21);
                    }
                    else
                    {
                        return sName.Substring(0, 21);
                    }
                }
                else
                    return sName.Substring(0, 21);
            }
            set
            {
                sName = value;
            }
        }
        // end name

        /// <summary>
        /// 
        /// </summary>
        public string RoutingNumber
        {
            get
            {
                if (sRoutingNumber.Length != 9)
                {
                    if (sRoutingNumber.Length < 9)
                    {
                        do
                        {
                            sRoutingNumber = sRoutingNumber + "0";
                        } while (sRoutingNumber.Length < 9);
                        return sRoutingNumber;
                    }
                    else if (sRoutingNumber.Length > 9)
                    {
                        return sRoutingNumber.Substring(0, 9);
                    }
                    else
                        return sRoutingNumber;
                }
                else
                    return sRoutingNumber;
            }
            set
            {
                sRoutingNumber = value;
            }
        }
        // end RountingNUmber


        /// <summary>
        /// 
        /// </summary>
        public string AccountNumber
        {
            get
            {

                if (sAccountNumber.Length != 17)
                {
                    if (sAccountNumber.Length < 17)
                    {
                        do
                        {
                            sAccountNumber = sAccountNumber + " ";
                        } while (sAccountNumber.Length < 17);
                        
                        return sAccountNumber;
                    }
                    else if (sAccountNumber.Length > 17)
                        return sAccountNumber.Substring(0, 17);
                    else
                        return sAccountNumber;

                }
                else
                    return sAccountNumber;
            }
            set
            {
                sAccountNumber = value;
            }

        }

    }
}
