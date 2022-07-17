using AssetsTools.NET;
using AssetsTools.NET.Extra;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using static KF3Parse;

public class AssetTool
{
    public AssetsManager am;

    private void ChangeFileVersion(int platformId, string input, string output, bool silent)
    {
        am = new AssetsManager();
        am.LoadClassPackage(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), @"classdata.tpk"));

        //Load file
        string selectedFile = input;
        BundleFileInstance bundleInst = null;
        try
        {
            bundleInst = am.LoadBundleFile(selectedFile, false);
            //Decompress the file to memory
            bundleInst.file = DecompressToMemory(bundleInst);
        }
        catch
        {
            if (!silent) Console.WriteLine($"Error: {Path.GetFileName(selectedFile)} is not a valid bundle file");
            return;
        }

        AssetsFileInstance inst = am.LoadAssetsFileFromBundle(bundleInst, 0);
        am.LoadClassDatabaseFromPackage(inst.file.typeTree.unityVersion);

        inst.file.typeTree.version = (uint)platformId; //5-pc //13-android //20-webgl

        //commit changes
        byte[] newAssetData;
        using (MemoryStream stream = new MemoryStream())
        {
            using (AssetsFileWriter writer = new AssetsFileWriter(stream))
            {
                inst.file.Write(writer, 0, new List<AssetsReplacer>() { }, 0);
                newAssetData = stream.ToArray();
            }
        }

        BundleReplacerFromMemory bunRepl = new BundleReplacerFromMemory(inst.name, null, true, newAssetData, -1);

        //write a modified file (temp)
        string tempFile = Path.GetTempFileName();
        using (var stream = File.OpenWrite(tempFile))
        using (var writer = new AssetsFileWriter(stream))
        {
            bundleInst.file.Write(writer, new List<BundleReplacer>() { bunRepl });
        }
        bundleInst.file.Close();

        //load the modified file for compression
        bundleInst = am.LoadBundleFile(tempFile);
        using (var stream = File.OpenWrite(output))
        using (var writer = new AssetsFileWriter(stream))
        {
            bundleInst.file.Pack(bundleInst.file.reader, writer, AssetBundleCompressionType.LZ4);
        }
        bundleInst.file.Close();

        File.Delete(tempFile);
        if (!silent) Console.WriteLine("complete");
        am.UnloadAll(); //delete this if something breaks
    }


    public void ReadParameterAsset(string filePath)
    {
        var local = KF3Parse.Instance;
        am = new AssetsManager();
        am.LoadClassPackage(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), @"classdata.tpk"));

        //Load file
        string selectedFile = filePath;
        BundleFileInstance bundleInst = null;
        try
        {
            bundleInst = am.LoadBundleFile(selectedFile, false);
            //Decompress the file to memory
            bundleInst.file = DecompressToMemory(bundleInst);
        }
        catch
        {
            Console.WriteLine($"Error: {Path.GetFileName(selectedFile)} is not a valid bundle file");
            return;
        }

        AssetsFileInstance inst = am.LoadAssetsFileFromBundle(bundleInst, 0);
        am.LoadClassDatabaseFromPackage(inst.file.typeTree.unityVersion);

        foreach (var inf in inst.table.assetFileInfo)
        {
            AssetTypeValueField baseField = am.GetTypeInstance(inst.file, inf).GetBaseField();
            var fileName = baseField["m_Name"].value.AsString();
            var dump = DumpJsonAsset(baseField);
            var split = fileName.Split("_");
            var id = -1;
            if (split.Length > 1) int.TryParse(split[1], out id);
            bool is2 = split.Length > 2;
            var charaData = local.CharaDatas.FirstOrDefault(c => c.id == id);
            switch (split[0])
            {
                case "ParamAbility":
                    {
                        var ability = JsonConvert.DeserializeObject<ParamAbility>(dump);
                        local.ParamAbilities.Add(ability);
                        if (charaData != null)
                        {
                            if (is2)
                                charaData.ParamAbility2 = ability;
                            else
                                charaData.ParamAbility = ability;
                        }
                        else
                        {
                            var photoData = local.PhotoDatas.FirstOrDefault(c => c.id == id);
                            if(photoData != null)
                            {
                                if (is2)
                                    photoData.postEffect = ability;
                                else
                                    photoData.preEffect = ability;
                            }
                        }
                        break;
                    }
                case "ParamAlphaBase":
                    {
                        var alpha = JsonConvert.DeserializeObject<ParamAlphaBase>(dump);
                        local.ParamAlphaBases.Add(alpha);
                        if (charaData != null) charaData.ParamAlphaBase = alpha;
                        break;
                    }
                case "ParamArts":
                    {
                        var art = JsonConvert.DeserializeObject<ParamArts>(dump);
                        local.ParamArts1.Add(art);
                        if (charaData != null) charaData.ParamArts = art;
                        break;
                    }
                case "ParamSpecialAttack":
                    {
                        var spec = JsonConvert.DeserializeObject<ParamSpecialAttack>(dump);
                        local.ParamSpecialAttacks.Add(spec);
                        if (charaData != null) charaData.ParamSpecialAttack = spec;
                        break;
                    }
                case "ParamWaitAction":
                    {
                        var wait = JsonConvert.DeserializeObject<ParamWaitAction>(dump);
                        local.ParamWaits.Add(wait);
                        if (charaData != null) charaData.ParamWaitAction = wait;
                        break;
                    }

                case "ParamEnemyBase":
                    {
                        var enemyData = local.QuestEnemyDatas.FirstOrDefault(c => c.enemyCharaId == id);
                        var alpha = JsonConvert.DeserializeObject<ParamEnemyBase>(dump);
                        if (enemyData != null) enemyData.ParamEnemyBase = alpha;
                        break;
                    }
                default:
                    break;
            }
            //else Console.WriteLine(fileName);
        }

        bundleInst.file.Close();
        am.UnloadAll(); //delete this if something breaks
    }

    public static string DumpJsonAsset(AssetTypeValueField baseField)
    {
        JToken jBaseField = RecurseJsonDump(baseField, false);
        string json = JsonConvert.SerializeObject(JObject.Parse(jBaseField.ToString()));
        json = json.Replace("{\"Array\":[", "[").Replace("]},", "],");
        json = json.Remove(json.Length - 1, 1);
        if (!json.EndsWith("}")) json += "}";
        return json;
    }

    private static JToken RecurseJsonDump(AssetTypeValueField field, bool uabeFlavor)
    {
        AssetTypeTemplateField template = field.GetTemplateField();

        bool isArray = template.isArray;

        if (isArray)
        {
            JArray jArray = new JArray();

            if (template.valueType != EnumValueTypes.ByteArray)
            {
                for (int i = 0; i < field.childrenCount; i++)
                {
                    jArray.Add(RecurseJsonDump(field.children[i], uabeFlavor));
                }
            }
            else
            {
                byte[] byteArrayData = field.GetValue().AsByteArray().data;
                for (int i = 0; i < byteArrayData.Length; i++)
                {
                    jArray.Add(byteArrayData[i]);
                }
            }

            return jArray;
        }
        else
        {
            if (field.GetValue() != null)
            {
                EnumValueTypes evt = field.GetValue().GetValueType();
                object value;
                switch (evt)
                {
                    case EnumValueTypes.Bool:
                        value = field.GetValue().AsBool(); break;
                    case EnumValueTypes.Int8:
                    case EnumValueTypes.Int16:
                    case EnumValueTypes.Int32: value = field.GetValue().AsInt(); break;
                    case EnumValueTypes.Int64: value = field.GetValue().AsInt64(); break;
                    case EnumValueTypes.UInt8:
                    case EnumValueTypes.UInt16:
                    case EnumValueTypes.UInt32: value = field.GetValue().AsUInt(); break;
                    case EnumValueTypes.UInt64: value = field.GetValue().AsUInt64(); break;
                    case EnumValueTypes.String: value = field.GetValue().AsString(); break;
                    case EnumValueTypes.Float: value = field.GetValue().AsFloat(); break;
                    case EnumValueTypes.Double: value = field.GetValue().AsDouble(); break;
                    default: value = "invalid"; break;
                }

                return (JValue)JToken.FromObject(value);
            }
            else
            {
                JObject jObject = new JObject();

                for (int i = 0; i < field.childrenCount; i++)
                {
                    jObject.Add(field.children[i].GetName(), RecurseJsonDump(field.children[i], uabeFlavor));
                }

                return jObject;
            }
        }
    }

    private static AssetBundleFile DecompressToMemory(BundleFileInstance bundleInst)
    {
        AssetBundleFile bundle = bundleInst.file;

        MemoryStream bundleStream = new MemoryStream();
        bundle.Unpack(bundle.reader, new AssetsFileWriter(bundleStream));

        bundleStream.Position = 0;

        AssetBundleFile newBundle = new AssetBundleFile();
        newBundle.Read(new AssetsFileReader(bundleStream), false);

        bundle.reader.Close();
        return newBundle;
    }
}
