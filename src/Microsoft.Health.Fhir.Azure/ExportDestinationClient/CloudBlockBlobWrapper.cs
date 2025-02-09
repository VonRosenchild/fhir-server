﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EnsureThat;
using Microsoft.Azure.Storage.Blob;

namespace Microsoft.Health.Fhir.Azure.ExportDestinationClient
{
    /// <summary>
    /// A wrapper class around <see cref="CloudBlockBlob"/> that also keeps track of the list of existing
    /// block ids in the current blob. Every time we want to update the blob data, we need to commit the list
    /// of new as well as existing block ids we want to keep. Hence the need for tracking block ids instead of
    /// getting it everytime from the blob itself.
    /// </summary>
    public class CloudBlockBlobWrapper
    {
        private readonly List<string> _existingBlockIds;
        private readonly CloudBlockBlob _cloudBlob;

        public CloudBlockBlobWrapper(CloudBlockBlob blockBlob)
        {
            EnsureArg.IsNotNull(blockBlob, nameof(blockBlob));

            _cloudBlob = blockBlob;

            // For now we assume that the blockblob we are using is always new. If it is an existing one,
            // we will have to get the existing list of block ids.
            _existingBlockIds = new List<string>();
        }

        public async Task UploadBlockAsync(string blockId, Stream data, string md5Hash, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotNullOrWhiteSpace(blockId, nameof(blockId));
            EnsureArg.IsNotNull(data, nameof(data));

            _existingBlockIds.Add(blockId);

            await _cloudBlob.PutBlockAsync(blockId, data, md5Hash, cancellationToken);
        }

        public async Task CommitBlockListAsync(CancellationToken cancellationToken)
        {
            await _cloudBlob.PutBlockListAsync(_existingBlockIds, cancellationToken);
        }
    }
}
