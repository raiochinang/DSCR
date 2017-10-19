using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.ServiceModel.Web;
using MyDownloader.Core;

namespace HOORESTService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "HOOAPI" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select HOOAPI.svc or HOOAPI.svc.cs at the Solution Explorer and start debugging.
    public class HOOAPI : IHOOAPI
    {
        public List<Branch> BranchList()
        {
            return Branches.Instance.BranchList;
        }

        public List<Check> CheckList(string rr_number)
        {
            return Checks.Instance.CheckList(rr_number);
        }

        public bool Connection()
        {
            return Particulars.Instance.Connection();
        }

        public List<CreditCard> CreditCardList(string rr_number)
        {
            return CreditCards.Instance.CreditCardList(rr_number);
        }

        public List<Person> Doctors(string name)
        {
            return Persons.Instance.Doctors(name);
        }

        public void DoWork()
        {
        }

        public DSCR dscr(DSCR dscr)
        {
            return DSCRs.Instance.GetDSCR(dscr);
        }

        public string DSCRPrint(DSCR dscr)
        {
            return DSCRs.Instance.DSCRPrint(dscr);
        }

        public DSCR dscrByDateRange(DSCR dscr)
        {
            return DSCRs.Instance.GetDSCRbyRange(dscr);
        }

        public List<Particular> GetParticularList()
        {
            return Particulars.Instance.ParticularList;
        }

        public List<Person> NurseList(string name)
        {
            return Persons.Instance.Nurses(name);
        }

        public DSCR DSCRInsert(DSCR dscr)
        {
            return DSCRs.Instance.Update(dscr);
        }

        public DSCR DSCRUpdate(DSCR dscr)
        {
            return DSCRs.Instance.Update(dscr);

        }

        public List<Particular> SearchParticulars(string barcode)
        {
            return Particulars.Instance.SearchParticulars(barcode);
        }


        public List<User> UserList()
        {
            return Users.Instance.UserList;
        }


        public List<User> UserSave(User user)
        {
            return Users.Instance.UserSave(user);
        }

        public Person Login(Person person)
        {
            return Persons.Instance.Login(person);
        }

        public List<Person> PatientList(string name)
        {
            return Persons.Instance.Patients(name);
        }


        public List<Person> CashierList(string name)
        {
            return Persons.Instance.Cashiers(name);
        }

        public List<Person> DSCRUsers()
        {
            return Persons.Instance.DSCRUsers();
        }

        public bool UserUpdate(Person person)
        {
            return Persons.Instance.UpdateUser(person);
        }


        public string mdshare(DSCR param)
        {
            return DSCRs.Instance.MDShare(param);
        }

        public string SalesPerBranch(DSCR param)
        {
            return DSCRs.Instance.SalesReportPerBranch(param);
        }

        public bool RetrieveFile(string path)
        {
            return true;

        }

        public List<Purchase> PurchaseList()
        {            
            return Purchases.Instance.PurchaseList();         
        }

        public Purchase PurchaseDetails(string po_id)
        {
            return Purchases.Instance.FindOne(po_id);
        }


        public Purchase PurchasingInsert(Purchase p)
        {
            return Purchases.Instance.Insert(p);
        }


        public Purchase PurchasingUpdate(Purchase p)
        {
            return Purchases.Instance.Update(p);
        }

        public ObjRequest RequestList(string branch_id)
        {
            return Requests.Instance.List(branch_id);
        }

        public ObjRequest RequestFindOne(string request_id)
        {
            return Requests.Instance.FindOne(request_id);
        }

        public ObjRequest RequestInsert(ObjRequest p)
        {
            return Requests.Instance.Insert(p);
        }

        public ObjRequest RequestUpdate(ObjRequest p)
        {
            return Requests.Instance.Update(p);
        }

        public string RequestApprove(Person p)
        {
            return Requests.Instance.Approved(p);
        }
    }
}
