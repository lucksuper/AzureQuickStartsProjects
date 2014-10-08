﻿//----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System.Configuration;

namespace ComputeWebJobsSDKBlobStorage
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    //******************************************************************************************************
    // This will show you how to perform common scenarios using the Microsoft Azure Blob storage service using 
    // the Microsoft Azure WebJobs SDK. The scenarios covered include triggering a function when a new blob is detected
    // or updated in a blob container and bind the contents of the blob to BCL types so you can read/ write the contents.   
    // 
    // TODO: Create a Storage Account through the Portal or Visual Studio and provide your [AccountName] and 
    //       [AccountKey] in the App.Config http://go.microsoft.com/fwlink/?LinkId=325277            
    //*****************************************************************************************************
    class Program
    {
        static void Main()
        {
            CreateDemoData();

            JobHost host = new JobHost();
            host.RunAndBlock();
        }

        private static void CreateDemoData()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("input");
            container.CreateIfNotExists();

            CloudBlockBlob blob = container.GetBlockBlobReference("BlobOperations.txt");
            blob.UploadText("Hello world!");

            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("persons");
            queue.CreateIfNotExists();

            Person person = new Person()
            {
                Name = "John",
                Age = 42
            };

            queue.AddMessage(new CloudQueueMessage(JsonConvert.SerializeObject(person)));
        }
    }
}