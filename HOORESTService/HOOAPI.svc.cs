﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

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
    }
}
