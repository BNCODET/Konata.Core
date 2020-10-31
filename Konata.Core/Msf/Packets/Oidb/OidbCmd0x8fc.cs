﻿// This file is automatic generated by script.  
// DO NOT EDIT DIRECTLY.   

using System;
using Konata.Library.Protobuf;

namespace Konata.Msf.Packets.Oidb
{
    public abstract class OidbCmd0x8fc : OidbSSOPkg
    {
        internal OidbCmd0x8fc(uint svcType, ReqBody reqBody)

            : base(0x8fc, svcType, null, (ProtoTreeRoot root) =>
            {
                root.AddTree(reqBody.BuildTree());
            })
        {

        }

        public enum CardType : uint
        {
            TEXT = 1,
            XC = 2,
        }

        public class CardNameElem : OidbStruct
        {
            // 0x08
            public CardType? card_type;

            // 0x12
            public string value;

            public override void Write(ProtoTreeRoot root)
            {
                root.AddLeafVar("08", (uint)card_type);
                root.AddLeafString("12", value);
            }
        }

        public class ClientInfo : OidbStruct
        {
            // 0x08
            public uint? implat;

            // 0x12
            public string clientver;

            public override void Write(ProtoTreeRoot root)
            {
                root.AddLeafVar("08", implat);
                root.AddLeafString("12", clientver);
            }
        }

        public class CommCardNameBuf : OidbStruct
        {
            // 0x0A
            public RichCardNameElem rpt_rich_card_name;

            public override void Write(ProtoTreeRoot root)
            {
                root.AddTree("0A", rpt_rich_card_name.BuildTree());
            }
        }

        public class LevelName : OidbStruct
        {
            // 0x08
            public uint? level;

            // 0x12
            public string name;

            public override void Write(ProtoTreeRoot root)
            {
                root.AddLeafVar("08", level);
                root.AddLeafString("12", name);
            }
        }

        public class MemberInfo : OidbStruct
        {
            // 0x08
            public long? uin;

            // 0x10
            public uint? point;

            // 0x18
            public uint? active_day;

            // 0x20
            public uint? level;

            // 0x2A
            public string special_title;

            // 0x30
            public int? special_title_expire_time;

            // 0x3A
            public string uin_name;

            // 0x42
            public string member_card_name;

            // 0x4A
            public string phone;

            // 0x52
            public string email;

            // 0x5A
            public string remark;

            // 0x60
            public uint? gender;

            // 0x6A
            public string job;

            // 0x70
            public uint? tribe_level;

            // 0x78
            public uint? tribe_point;

            // 0x8201
            public CardNameElem rpt_rich_card_name;

            // 0x8A01
            public CommCardNameBuf comm_rich_card_name;

            public override void Write(ProtoTreeRoot root)
            {
                root.AddLeafVar("08", uin);
                root.AddLeafVar("10", point);
                root.AddLeafVar("18", active_day);
                root.AddLeafVar("20", level);
                root.AddLeafString("2A", special_title);
                root.AddLeafVar("30", special_title_expire_time);
                root.AddLeafString("3A", uin_name);
                root.AddLeafString("42", member_card_name);
                root.AddLeafString("4A", phone);
                root.AddLeafString("52", email);
                root.AddLeafString("5A", remark);
                root.AddLeafVar("60", gender);
                root.AddLeafString("6A", job);
                root.AddLeafVar("70", tribe_level);
                root.AddLeafVar("78", tribe_point);
                root.AddTree("8201", rpt_rich_card_name.BuildTree());
                root.AddTree("8A01", comm_rich_card_name.BuildTree());
            }
        }

        public class ReqBody : OidbStruct
        {
            // 0x08
            public long? group_code;

            // 0x10
            public uint? show_flag;

            // 0x1A
            public MemberInfo rpt_mem_level_info;

            // 0x22
            public LevelName rpt_level_name;

            // 0x28
            public uint? update_time;

            // 0x30
            public uint? office_mode;

            // 0x38
            public uint? group_open_appid;

            // 0x42
            public ClientInfo msg_client_info;

            // 0x4A
            public byte[] auth_key;

            public override void Write(ProtoTreeRoot root)
            {
                root.AddLeafVar("08", group_code);
                root.AddLeafVar("10", show_flag);
                root.AddTree("1A", rpt_mem_level_info.BuildTree());
                root.AddTree("22", rpt_level_name.BuildTree());
                root.AddLeafVar("28", update_time);
                root.AddLeafVar("30", office_mode);
                root.AddLeafVar("38", group_open_appid);
                root.AddTree("42", msg_client_info.BuildTree());
                root.AddLeafBytes("4A", auth_key);
            }
        }

        public class RichCardNameElem : OidbStruct
        {
            // 0x0A
            public byte[] ctrl;

            // 0x12
            public byte[] text;

            public override void Write(ProtoTreeRoot root)
            {
                root.AddLeafBytes("0A", ctrl);
                root.AddLeafBytes("12", text);
            }
        }

        public class RspBody : OidbStruct
        {
            // 0x08
            public long? group_code;

            // 0x12
            public string strErrInfo;

            public override void Write(ProtoTreeRoot root)
            {
                root.AddLeafVar("08", group_code);
                root.AddLeafString("12", strErrInfo);
            }
        }
    }
}
