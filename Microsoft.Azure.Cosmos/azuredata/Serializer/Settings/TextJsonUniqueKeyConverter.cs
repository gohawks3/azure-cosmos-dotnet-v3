﻿//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Azure.Cosmos
{
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Microsoft.Azure.Documents;

    internal class TextJsonUniqueKeyConverter : JsonConverter<UniqueKey>
    {
        public override UniqueKey Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException(string.Format(CultureInfo.CurrentCulture, RMResources.JsonUnexpectedToken));
            }

            using JsonDocument json = JsonDocument.ParseValue(ref reader);
            JsonElement root = json.RootElement;
            return TextJsonUniqueKeyConverter.ReadProperty(root);
        }

        public override void Write(
            Utf8JsonWriter writer,
            UniqueKey key,
            JsonSerializerOptions options)
        {
            TextJsonUniqueKeyConverter.WritePropertyValues(writer, key, options);
        }

        public static void WritePropertyValues(
            Utf8JsonWriter writer,
            UniqueKey key,
            JsonSerializerOptions options)
        {
            if (key == null)
            {
                return;
            }

            writer.WriteStartObject();

            if (key.Paths != null)
            {
                writer.WritePropertyName(JsonEncodedStrings.Paths);
                writer.WriteStartArray();
                foreach (string path in key.Paths)
                {
                    writer.WriteStringValue(path);
                }

                writer.WriteEndArray();
            }

            writer.WriteEndObject();
        }

        public static UniqueKey ReadProperty(JsonElement root)
        {
            UniqueKey key = new UniqueKey();
            if (root.TryGetProperty(JsonEncodedStrings.Paths.EncodedUtf8Bytes, out JsonElement pathsProperty))
            {
                key.Paths = new Collection<string>();
                foreach (JsonElement item in pathsProperty.EnumerateArray())
                {
                    key.Paths.Add(item.GetString());
                }
            }

            return key;
        }
    }
}