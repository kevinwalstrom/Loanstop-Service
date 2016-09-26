using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

using LoanStopModel;
using LoanStop.TeleTrackClient.Proxy;

namespace LoanStop.DepositService.Core
{
    public class ColoradoTeleTrack : ITeleTrack
    {

        public void Inquiry(Client client)
        {
            
            TransactionRequestEntity requestEntity = new TransactionRequestEntity();

            //subscriber
            SubscriberEntity subscriberEntity = new SubscriberEntity();
            subscriberEntity.UserName = "BnRCkH0ld";
            subscriberEntity.SubscriberID = "44343";
            requestEntity.Subscriber = subscriberEntity;
            
            //request details
            RequestEntity requestDetail = new RequestEntity();
            requestDetail.RequestType = RequestEntityRequestType.Inquiry;
            requestDetail.RequestOption = new SettingEntity[] { new SettingEntity(){Name = "Payday"} };
            requestEntity.RequestDetails = new RequestEntity[] {requestDetail};

            //customer information
            requestEntity.Applicant = new ConsumerEntity();
            requestEntity.Applicant.Name = new NameEntity();
            requestEntity.Applicant.Name.FirstName = client.Firstname;  
            requestEntity.Applicant.Name.LastName = client.Lastname;  
            requestEntity.Applicant.Name.MiddleName = client.Mi;
            requestEntity.Applicant.Address = new AddressEntity();
            requestEntity.Applicant.Address.SimpleAddress = new SimpleAddressEntity();
            requestEntity.Applicant.Address.SimpleAddress.Line1 = client.Address;
            requestEntity.Applicant.Address.SimpleAddress.City = client.City;
            requestEntity.Applicant.Address.SimpleAddress.State = client.State;
            requestEntity.Applicant.Address.SimpleAddress.PostalCode = client.Zip;
            requestEntity.Applicant.BirthDate = DateTime.Parse(((AuxClient)client.AuxClient).Dob);
            requestEntity.Applicant.DriversLicense = new DriversLicenseEntity();
            requestEntity.Applicant.DriversLicense.Number = client.DriverLicense;
            requestEntity.Applicant.DriversLicense.State = client.DriverLicense;
            var cellPhone = new PhoneEntity();
            cellPhone.Number = ((AuxClient)client.AuxClient).CellPhone;
            cellPhone.Type = PhoneTypeCodeType.Mobile;
            var workPhone = new PhoneEntity();
            workPhone.Number = client.WorkPhone;
            workPhone.Type = PhoneTypeCodeType.Work;
            var homePhone = new PhoneEntity();
            homePhone.Number = client.HomePhone;
            homePhone.Type = PhoneTypeCodeType.Home;
            requestEntity.Applicant.Phones = new PhoneEntity[]{cellPhone,workPhone,homePhone};

            //employmennt information
            EmployerEntity employerEntity = new EmployerEntity();
            employerEntity.EmployerName = client.Employer;
            employerEntity.Occupation = ((AuxClient)client.AuxClient).Occupation;
            IncomeEntity incomeEntity = new IncomeEntity();
            incomeEntity.Amount = decimal.Parse(((AuxClient)client.AuxClient).NetFromPaystubs);
            incomeEntity.PaymentFrequency = PaymentFrequencyType.Biweekly;
            employerEntity.Salary = incomeEntity;

            requestEntity.Applicant.Employer = employerEntity;

            InquirySoapClient teleTrack = new InquirySoapClient();

            teleTrack.ClientCredentials.UserName.UserName = @"BnRCkH0ld";
            teleTrack.ClientCredentials.UserName.Password = @"8ed59093-3";

            var response = teleTrack.GetData(requestEntity);
        
        }


        public long IssueLoan()
        {

            long rtrn = 0;

            return rtrn;
        }

        public long PartialPayment()
        {
            long rtrn = 0;

            return rtrn;
        }

        public long PaidInFull()
        {
            long rtrn = 0;

            return rtrn;
        }
   
    }
}

