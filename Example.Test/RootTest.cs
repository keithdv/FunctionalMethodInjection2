﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac;
using ObjectPortal;
using Example.Lib;
using Example.DalConcrete;
using Example.Dal;

namespace Example.Test
{
    [TestClass]
    public class RootTest
    {

        static IContainer container;
        ILifetimeScope scope;


        [TestInitialize]
        public void TestInitialize()
        {

            if(container == null)
            {

                ContainerBuilder builder = new ContainerBuilder();

                builder.RegisterModule<ObjectPortal.AutofacModule>();


                builder.RegisterType<RootDal>().AsImplementedInterfaces();
                builder.RegisterType<BusinessItemDal>().AsImplementedInterfaces();
                builder.RegisterModule<LibModule>();


                container = builder.Build();

            }

            scope = container.BeginLifetimeScope();

        }

        [TestMethod]
        public void Root_Create()
        {
            
            var portal = scope.Resolve<IObjectPortal<IRoot>>();

            var result = portal.Create();
            
            Assert.AreEqual(1, result.BusinessItemList.Count);
            Assert.IsTrue(result.IsNew);
        }


        [TestMethod]
        public void Root_CreateCriteria()
        {

            var portal = scope.Resolve<IObjectPortal<IRoot>>();
            var criteria = Guid.NewGuid();
            var result = portal.Create(criteria);

            Assert.IsTrue(result.IsNew);
            Assert.AreEqual(1, result.BusinessItemList.Count);
            Assert.AreEqual(criteria, result.BusinessItemList[0].Criteria);

        }


        [TestMethod]
        public void Root_Fetch()
        {


            var portal = scope.Resolve<IObjectPortal<IRoot>>();

            var result = portal.Fetch();

            Assert.IsNotNull(result.BusinessItemList);
            Assert.AreEqual(2, result.BusinessItemList.Count);
        }

        [TestMethod]
        public void Root_Fetch_Criteria()
        {
            var portal = scope.Resolve<IObjectPortal<IRoot>>();
            var criteria = Guid.NewGuid();

            var result = portal.Fetch(criteria);

            Assert.AreEqual(criteria, result.BusinessItemList[0].Criteria);
            Assert.AreEqual(Guid.Empty, result.BusinessItemList[0].UpdatedID);

        }

        [TestMethod]
        public void Root_BusinessRule()
        {

            var portal = scope.Resolve<IObjectPortal<IRoot>>();

            var result = portal.Create();

            result.BusinessItemList[0].Name = Guid.NewGuid().ToString();

            Assert.IsTrue(result.IsValid);

        }

        [TestMethod]
        public void Root_Update()
        {
            Assert.Fail();
            //var portal = scope.Resolve<FetchRoot>();
            //var result = portal();

            //var update = scope.Resolve<Update<IRoot>>();

            //update(result);


            //Assert.AreNotEqual(Guid.Empty, result.BusinessItemList[0].UpdatedID);

        }


        //[TestMethod]
        //public void AddChild()
        //{
        //    var fetchRoot = scope.Resolve<FetchRoot>();

        //    var result = fetchRoot();

        //    var count = result.BusinessItemList.Count;

        //    result.BusinessItemList.AddChild();

        //    Assert.AreEqual(count + 1, result.BusinessItemList.Count);
        //}

        //[TestMethod]
        //public void AddCreateChild()
        //{
        //    var fetchRoot = scope.Resolve<FetchRoot>();

        //    var result = fetchRoot();

        //    var count = result.BusinessItemList.Count;

        //    var newBo = result.BusinessItemList.AddCreateChild();

        //    Assert.IsNotNull(newBo);
        //    Assert.AreEqual(count + 1, result.BusinessItemList.Count);

        //}

        //[TestMethod]
        //public void AddCreateChildWCriteria()
        //{
        //    var fetchRoot = scope.Resolve<FetchRoot>();

        //    var result = fetchRoot();

        //    var count = result.BusinessItemList.Count;
        //    var criteria = Guid.NewGuid();
        //    var newBo = result.BusinessItemList.AddCreateChild(criteria);

        //    Assert.IsNotNull(newBo);
        //    Assert.AreEqual(count + 1, result.BusinessItemList.Count);
        //    Assert.AreEqual(criteria, newBo.Criteria);

        //}

    }
}
