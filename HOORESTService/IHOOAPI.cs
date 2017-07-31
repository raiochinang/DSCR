﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace HOORESTService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IHOOAPI" in both code and config file together.
    [ServiceContract]
    public interface IHOOAPI
    {
        [OperationContract]
        void DoWork();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
                                   BodyStyle = WebMessageBodyStyle.Wrapped,
                                   UriTemplate = "Particulars/")]
        List<Particular> GetParticularList();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
                                   BodyStyle = WebMessageBodyStyle.Wrapped,
                                   UriTemplate = "Particulars/{barcode}")]
        List<Particular> SearchParticulars(string barcode);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
                                   BodyStyle = WebMessageBodyStyle.Wrapped,
                                   UriTemplate = "Connection/")]
        bool Connection();


        #region Checks
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
                              BodyStyle = WebMessageBodyStyle.Wrapped,
                              UriTemplate = "Check/{rr_number}")]
        List<Check> CheckList(string rr_number);
        #endregion

        #region Credit
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
                              BodyStyle = WebMessageBodyStyle.Wrapped,
                              UriTemplate = "CreditCard/{rr_number}")]
        List<CreditCard> CreditCardList(string rr_number);
        #endregion

        #region DSCR
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json,
                             RequestFormat = WebMessageFormat.Json,
                             BodyStyle = WebMessageBodyStyle.Bare,
                             UriTemplate = "DSCR/")]
        DSCR dscr(DSCR dscr);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json,
                             RequestFormat = WebMessageFormat.Json,
                             BodyStyle = WebMessageBodyStyle.Bare,
                             UriTemplate = "DSCR/ByDateRange")]
        DSCR dscrByDateRange(DSCR dscr);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json,
                             RequestFormat = WebMessageFormat.Json,
                             BodyStyle = WebMessageBodyStyle.Bare,
                             UriTemplate = "DSCR/Insert")]
        DSCR DSCRInsert(DSCR dscr);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json,
                             RequestFormat = WebMessageFormat.Json,
                             BodyStyle = WebMessageBodyStyle.Bare,
                             UriTemplate = "DSCR/Update")]
        DSCR DSCRUpdate(DSCR dscr);
        #endregion

        #region Branch
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
                                    BodyStyle = WebMessageBodyStyle.Wrapped,
                                    UriTemplate = "Branch/")]
        List<Branch> BranchList();
        #endregion Branch

        #region Person
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
                             BodyStyle = WebMessageBodyStyle.Wrapped,
                             UriTemplate = "Doctor/{name}")]
        List<Person> Doctors(string name);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
                             BodyStyle = WebMessageBodyStyle.Wrapped,
                             UriTemplate = "Nurse/{name}")]
        List<Person> NurseList(string name);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
                             BodyStyle = WebMessageBodyStyle.Wrapped,
                             UriTemplate = "Cashier/{name}")]
        List<Person> CashierList(string name);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
                             BodyStyle = WebMessageBodyStyle.Wrapped,
                             UriTemplate = "Patient/{name}")]
        List<Person> PatientList(string name);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json,
                            RequestFormat = WebMessageFormat.Json,
                            BodyStyle = WebMessageBodyStyle.Bare,
                            UriTemplate = "Login/")]
        Person Login(Person person);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
                            BodyStyle = WebMessageBodyStyle.Wrapped,
                            UriTemplate = "admin/")]
        List<Person> DSCRUsers();

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json,
                            RequestFormat = WebMessageFormat.Json,
                            BodyStyle = WebMessageBodyStyle.Bare,
                            UriTemplate = "admin/update")]
        bool UserUpdate(Person person);

        #endregion Person

        #region Users
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
                             BodyStyle = WebMessageBodyStyle.Wrapped,
                             UriTemplate = "Users/")]
        List<User> UserList();

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json,
                             RequestFormat = WebMessageFormat.Json,
                             BodyStyle = WebMessageBodyStyle.Bare,
                             UriTemplate = "Users/Save")]
        List<User> UserSave(User user);
        #endregion Users

        #region "Generator"
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json,
                             RequestFormat = WebMessageFormat.Json,
                             BodyStyle = WebMessageBodyStyle.Bare,
                             UriTemplate = "MDShare/")]
        bool mdshare(DSCR param);
        #endregion
    }
}
