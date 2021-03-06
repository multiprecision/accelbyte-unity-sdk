﻿// Copyright (c) 2018 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.IntegrationTests
{
	namespace EcommerceTest
	{
		[TestFixture]
    	public class GetCategory
    	{
    		[UnityTest]
    		public IEnumerator GetCategory_CategoryValid_Success()
    		{
    			Categories categories = AccelBytePlugin.GetCategories();
    			Result<Category> getCategoryResult = null;
    			
			    categories.GetCategory(TestVariables.rootCategoryPath, TestVariables.language, result => { getCategoryResult = result; });
    			while (getCategoryResult == null) { yield return new WaitForSeconds(0.1f); }
    			
    			TestHelper.Assert(() => Assert.IsTrue(!getCategoryResult.IsError, "Get category failed."));
    		}
		    
		    [UnityTest]
    		public IEnumerator GetCategory_CategoryInvalid_NotFound()
    		{
			    Categories categories = AccelBytePlugin.GetCategories();
    			const string invalidCategoryPath = "/invalidPath";
    			Result<Category> getCategoryResult = null;
    			
    			categories.GetCategory(invalidCategoryPath, TestVariables.language, result => { getCategoryResult = result; });
    			while (getCategoryResult == null) { yield return new WaitForSeconds(0.1f); }
    			
			    TestHelper.Assert(() => Assert.IsTrue(getCategoryResult.IsError, "Get invalid category failed."));
    			TestHelper.Assert(() => Assert.IsTrue(getCategoryResult.Error.Code.Equals(ErrorCode.CategoryNotFound), 
	                "Get invalid category failed."));
    		}
		    
		    [UnityTest]
		    public IEnumerator GetCategory_CategoryEmpty_InvalidRequest()
		    {
			    Categories categories = AccelBytePlugin.GetCategories();
			    const string invalidCategoryPath = "invalid";
			    Result<Category> getCategoryResult = null;
    			
			    categories.GetCategory(invalidCategoryPath, TestVariables.language, result => { getCategoryResult = result; });
			    while (getCategoryResult == null) { yield return new WaitForSeconds(0.1f); }
			    
			    TestHelper.Assert(() => Assert.IsTrue(!getCategoryResult.Error.Code.Equals(ErrorCode.BadRequest), 
				    "Get invalid category failed."));
		    }
		    
		    [UnityTest]
    		public IEnumerator GetCategory_LanguageInvalid_Success()
    		{
			    Categories categories = AccelBytePlugin.GetCategories();
			    const string invalidLanguage = "english";
			    Result<Category> getCategoryResult = null;
    			
    			categories.GetCategory(TestVariables.rootCategoryPath, invalidLanguage, result => { getCategoryResult = result; });
    			while (getCategoryResult == null) { yield return new WaitForSeconds(0.1f); }
    			
    			TestHelper.Assert(() => Assert.IsTrue(!getCategoryResult.IsError, "Get category with invalid language failed."));
    		}
		    		    
		    [UnityTest]
		    public IEnumerator GetCategory_LanguageEmpty_Fail()
		    {
			    Categories categories = AccelBytePlugin.GetCategories();
			    Result<Category> getCategoryResult = null;
    			
			    categories.GetCategory(TestVariables.rootCategoryPath, "", result => { getCategoryResult = result; });
			    while (getCategoryResult == null) { yield return new WaitForSeconds(0.1f); }
    			
			    TestHelper.Assert(() => Assert.IsTrue(getCategoryResult.IsError, "Get category with invalid language should be failed."));
		    }
    	}

		[TestFixture]
		public class GetRootCategory
		{
			[UnityTest]
			public IEnumerator GetRootCategory_LanguageValid_Success()
			{
				Categories categories = AccelBytePlugin.GetCategories();
				Result<Category[]> getRootCategoryResult = null;
    			
				categories.GetRootCategories(TestVariables.rootCategoryPath, result => { getRootCategoryResult = result; });
				while (getRootCategoryResult == null) { yield return new WaitForSeconds(0.1f); }
    
				TestHelper.Assert(() => Assert.IsTrue(!getRootCategoryResult.IsError, "Get root category failed."));
				TestHelper.Assert(() => Assert.IsNotNull(getRootCategoryResult.Value, "Get root category return empty."));
			}
			
			[UnityTest]
			public IEnumerator GetRootCategory_LanguageInvalid_Success()
			{
				Categories categories = AccelBytePlugin.GetCategories();
				const string invalidLanguage = "unknown";
				Result<Category[]> getRootCategoryResult = null;
    			
				categories.GetRootCategories(invalidLanguage, result => { getRootCategoryResult = result; });
				while (getRootCategoryResult == null) { yield return new WaitForSeconds(0.1f); }
    
				TestHelper.Assert(() => Assert.IsTrue(!getRootCategoryResult.IsError, "Get root category with invalid language failed."));
				TestHelper.Assert(() => Assert.IsNotNull(getRootCategoryResult.Value, "Get root category with invalid language return empty."));
			}
			
			[UnityTest]
			public IEnumerator GetRootCategory_LanguageEmpty_Fail()
			{
				Categories categories = AccelBytePlugin.GetCategories();
				const string emptyLanguage = "";
				Result<Category[]> getRootCategoryResult = null;
    			
				categories.GetRootCategories(emptyLanguage, result => { getRootCategoryResult = result; });
				while (getRootCategoryResult == null) { yield return new WaitForSeconds(0.1f); }
    
				TestHelper.Assert(() => Assert.That(getRootCategoryResult.IsError, "Get root category with empty language not failed."));
			}
		}

		[TestFixture]
		public class GetChildCategory
		{
			[UnityTest]
			public IEnumerator GetChildCategory_CategoryValid_Success()
			{
				Categories categories = AccelBytePlugin.GetCategories();
				Result<Category[]> getChildCategoryResult = null;
				bool containDogeCoin = false;
    			
				categories.GetChildCategories(TestVariables.rootCategoryPath, TestVariables.language, result => { getChildCategoryResult = result; });
				while (getChildCategoryResult == null) { yield return new WaitForSeconds(0.1f); }
    
				foreach (Category child in getChildCategoryResult.Value)
				{
					if (child.categoryPath.Contains(TestVariables.childCategoryPath))
					{
						containDogeCoin = true;
					}
				}
    			
				TestHelper.Assert(() => Assert.IsTrue(!getChildCategoryResult.IsError, "Get child category failed."));			
				TestHelper.Assert(() => Assert.IsTrue(containDogeCoin, "Get child category failed."));
			}
			
			[UnityTest]
			public IEnumerator GetChildCategory_CategoryInvalid_ReturnAnEmptyArray()
			{
				Categories categories = AccelBytePlugin.GetCategories();
				const string invalidCategoryPath = "/invalidPath";
				Result<Category[]> getChildCategoryResult = null;
    			
				categories.GetChildCategories(invalidCategoryPath, TestVariables.language, result => { getChildCategoryResult = result; });
				while (getChildCategoryResult == null) { yield return new WaitForSeconds(0.1f); }
    			
				TestHelper.Assert(() => Assert.IsTrue(!getChildCategoryResult.IsError, "Get child category with invalid path not return an empty array."));			
				TestHelper.Assert(() => Assert.IsTrue(getChildCategoryResult.Value.Length == 0, "Get child category with invalid path not return an empty array."));
			}
						
			[UnityTest]
			public IEnumerator GetChildCategory_CategoryEmpty_Error()
			{
				Categories categories = AccelBytePlugin.GetCategories();
				const string emptyCategoryPath = "";
				Result<Category[]> getChildCategoryResult = null;
    			
				categories.GetChildCategories(emptyCategoryPath, TestVariables.language, result => { getChildCategoryResult = result; });
				while (getChildCategoryResult == null) { yield return new WaitForSeconds(0.1f); }
    			
				TestHelper.Assert(() => Assert.IsTrue(getChildCategoryResult.IsError, "Get child category with empty path not error."));			
				TestHelper.Assert(() => Assert.IsNull(getChildCategoryResult.Value, "Get child category with empty path return a data."));
			}
			
			[UnityTest]
			public IEnumerator GetChildCategory_CategoryValid_LanguageInvalid_Success()
			{
				Categories categories = AccelBytePlugin.GetCategories();
				const string invalidLanguage = "unknown";
				Result<Category[]> getChildCategoryResult = null;
				bool containDogeCoin = false;
				
				categories.GetChildCategories(TestVariables.rootCategoryPath, invalidLanguage, result => { getChildCategoryResult = result; });
				while (getChildCategoryResult == null) { yield return new WaitForSeconds(0.1f); }
    
				foreach (Category child in getChildCategoryResult.Value)
				{
					if (child.categoryPath.Contains(TestVariables.childCategoryPath))
					{
						containDogeCoin = true;
					}
				}
    			
				TestHelper.Assert(() => Assert.IsTrue(!getChildCategoryResult.IsError, "Get child category with invalid language failed."));			
				TestHelper.Assert(() => Assert.IsTrue(containDogeCoin, "Get child category with invalid language failed."));
			}
			
			[UnityTest]
			public IEnumerator GetChildCategory_CategoryValid_LanguageEmpty_Success()
			{
				Categories categories = AccelBytePlugin.GetCategories();
				const string emptyLanguage  = "";
				Result<Category[]> getChildCategoryResult = null;
				bool containDogeCoin = false;
    			
				categories.GetChildCategories(TestVariables.rootCategoryPath, emptyLanguage, result => { getChildCategoryResult = result; });
				while (getChildCategoryResult == null) { yield return new WaitForSeconds(0.1f); }
    			
				TestHelper.Assert(() => Assert.IsTrue(getChildCategoryResult.IsError, "Get child category with empty language not failed."));			
			}
		}

		[TestFixture]
		public class GetDescendantCategory
		{
			[UnityTest]
			public IEnumerator GetDescendantCategory_CategoryValid_Success()
			{
				var user = AccelBytePlugin.GetUser();
				Result loginResult = null;
				user.LoginWithDeviceId(result => loginResult = result);
				
				while (loginResult == null) { yield return new WaitForSeconds(0.1f); }
				
				Categories categories = AccelBytePlugin.GetCategories();
				Result<Category[]> getDescendantCategoryResult = null;
				bool containDogeCoin = false;
    			
				categories.GetDescendantCategories(TestVariables.rootCategoryPath, TestVariables.language, result => { getDescendantCategoryResult = result; });
				while (getDescendantCategoryResult == null) { yield return new WaitForSeconds(0.1f); }
				
				foreach (Category child in getDescendantCategoryResult.Value)
				{
					if (child.categoryPath.Contains(TestVariables.grandChildCategoryPath))
					{
						containDogeCoin = true;
					}
				}
				
				TestHelper.Assert(() => Assert.IsTrue(!getDescendantCategoryResult.IsError, "Get descendant category failed."));
				TestHelper.Assert(() => Assert.IsTrue(containDogeCoin, "Get descendant category failed."));
			}
			
			[UnityTest]
			public IEnumerator GetDescendantCategory_CategoryInvalid_ReturnAnEmptyArray()
			{
				Categories categories = AccelBytePlugin.GetCategories();
				Result<Category[]> getDescendantCategoryResult = null;
				const string invalidCategory = "invalid";
				
				categories.GetDescendantCategories(invalidCategory, TestVariables.language, result => { getDescendantCategoryResult = result; });
				while (getDescendantCategoryResult == null) { yield return new WaitForSeconds(0.1f); }

				TestHelper.Assert(() => Assert.IsTrue(!getDescendantCategoryResult.IsError, "Get descendant category with invalid category failed."));
				TestHelper.Assert(() => Assert.IsTrue(getDescendantCategoryResult.Value.Length == 0, "Get descendant category with invalid category not return an empty array."));
			}
						
			[UnityTest]
			public IEnumerator GetDescendantCategory_CategoryEmpty_Error()
			{
				Categories categories = AccelBytePlugin.GetCategories();
				Result<Category[]> getDescendantCategoryResult = null;
    			
				categories.GetDescendantCategories("", TestVariables.language, result => { getDescendantCategoryResult = result; });
				while (getDescendantCategoryResult == null) { yield return new WaitForSeconds(0.1f); }

				TestHelper.Assert(() => Assert.IsTrue(getDescendantCategoryResult.IsError, "Get descendant category with empty category failed."));
				TestHelper.Assert(() => Assert.IsNull(getDescendantCategoryResult.Value, "Get descendant category with empty category not return a data."));
			}
			
			[UnityTest]
			public IEnumerator GetDescendantCategory_CategoryValid_LanguageInvalid_Success()
			{
				var user = AccelBytePlugin.GetUser();
				Result loginResult = null;
				user.LoginWithDeviceId(result => loginResult = result);
				
				while (loginResult == null) { yield return new WaitForSeconds(0.1f); }
				
				Categories categories = AccelBytePlugin.GetCategories();
				Result<Category[]> getDescendantCategoryResult = null;
				bool containDogeCoin = false;
    			
				categories.GetDescendantCategories(TestVariables.rootCategoryPath, "unknown", result => { getDescendantCategoryResult = result; });
				while (getDescendantCategoryResult == null) { yield return new WaitForSeconds(0.1f); }
				
				foreach (Category child in getDescendantCategoryResult.Value)
				{
					if (child.categoryPath.Contains(TestVariables.grandChildCategoryPath))
					{
						containDogeCoin = true;
					}
				}
				
				TestHelper.Assert(() => Assert.IsTrue(!getDescendantCategoryResult.IsError, "Get descendant category with invalid language failed."));
				TestHelper.Assert(() => Assert.IsTrue(containDogeCoin, "Get descendant category with invalid language failed."));
			}
			
			[UnityTest]
			public IEnumerator GetDescendantCategory_CategoryValid_LanguageEmpty_Fail()
			{
				Categories categories = AccelBytePlugin.GetCategories();
				Result<Category[]> getDescendantCategoryResult = null;
				bool containDogeCoin = false;
				
				categories.GetDescendantCategories(TestVariables.rootCategoryPath, "", result => { getDescendantCategoryResult = result; });
				while (getDescendantCategoryResult == null) { yield return new WaitForSeconds(0.1f); }
				
				TestHelper.Assert(() => Assert.IsTrue(getDescendantCategoryResult.IsError, "Get descendant category with empty language not failed."));
			}
		}	
	}
}	
