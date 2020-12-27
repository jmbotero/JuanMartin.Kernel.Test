﻿using JuanMartin.Kernel.Messaging;
using JuanMartin.Kernel.Test.Adapters;
using NUnit.Framework;

namespace JuanMartin.Kernel.Adapters.Tests
{
    [TestFixture]
    public class AdapterMySqlTests
    {
        [Test]
        public static void MessageRequestReplyTest()
        {
            AdapterMySqlMock adapter = new AdapterMySqlMock();

            Message request = new Message("Command", System.Data.CommandType.StoredProcedure.ToString());

            request.AddData("uspAdapterTest");
            request.AddSender("MysqlTest", typeof(AdapterMySqlTests).ToString());

            adapter.Send(request);
            IRecordSet reply = (IRecordSet)adapter.Receive();

            Assert.AreNotEqual(reply.Data.Annotations.Count, 0);
            Assert.AreEqual(reply.Data.GetAnnotationByValue(1).GetAnnotation("id").Value, 1);
        }
    }
}
