﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using EnsureThat;
using Newtonsoft.Json;

namespace Microsoft.Health.Fhir.Core.Features.Operations.Export.Models
{
    public class ExportFileInfo
    {
        public ExportFileInfo(
            string type,
            Uri fileUri,
            int sequence,
            int count,
            long committedBytes)
        {
            EnsureArg.IsNotNullOrEmpty(type);
            EnsureArg.IsNotNull(fileUri);

            Type = type;
            FileUri = fileUri;
            Sequence = sequence;
            Count = count;
            CommittedBytes = committedBytes;
        }

        [JsonConstructor]
        protected ExportFileInfo()
        {
        }

        [JsonProperty(JobRecordProperties.Type)]
        public string Type { get; private set; }

        [JsonProperty(JobRecordProperties.Url)]
        public Uri FileUri { get; private set; }

        [JsonProperty(JobRecordProperties.Sequence)]
        public int Sequence { get; private set; }

        [JsonProperty(JobRecordProperties.Count)]
        public int Count { get; private set; }

        [JsonProperty(JobRecordProperties.CommitedBytes)]
        public long CommittedBytes { get; private set; }
    }
}
