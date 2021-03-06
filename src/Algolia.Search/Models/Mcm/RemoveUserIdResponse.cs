/*
* Copyright (c) 2018 Algolia
* http://www.algolia.com/
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using Algolia.Search.Exceptions;
using Algolia.Search.Models.Common;
using System;
using System.Threading.Tasks;

namespace Algolia.Search.Models.Mcm
{
    /// <summary>
    /// Waitable response for removeUserId
    /// </summary>
    public class RemoveUserIdResponse : IAlgoliaWaitableResponse
    {
        internal Func<string, RemoveUserIdResponse> RemoveUserId { get; set; }

        /// <summary>
        /// UserId to remove
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Date of deletion
        /// </summary>
        public DateTime DeletedAt { get; set; }

        /// <summary>
        /// As the delete operation is asynchronous
        /// Wait until the userId is deleted on the API
        /// </summary>
        public void Wait()
        {
            RemoveUserIdResponse deleteResponse;

            while (true)
            {
                try
                {
                    deleteResponse = RemoveUserId(UserId);
                }
                catch (AlgoliaApiException ex)
                {
                    // Loop until we don't have Error 400: "Another mapping operation is already running for this userID"
                    if (ex.Message.Contains("Another mapping operation is already running for this userID"))
                    {
                        Task.Delay(1000);
                        continue;
                    }

                    throw;
                }

                DeletedAt = deleteResponse.DeletedAt;
                break;
            }
        }
    }
}