﻿// Created: 2017/03/30 5:22 AM
// Updated: 2017/09/29 2:08 AM
// 
// Project: MapleLib
// Filename: MapleTicket.cs
// Created By: Jared T

using System.Collections.Generic;
using MapleLib.Common;
using MapleLib.Properties;
using MapleLib.Structs;

namespace MapleLib.WiiU
{
    public static class MapleTicket
    {
        private const int Tk = 0x140;

        private static void PatchDlc(ref List<byte> ticketData)
        {
            ticketData.InsertRange(Tk + 0x164, Resources.DLCPatch);
        }

        private static void PatchDemo(ref List<byte> ticketData)
        {
            ticketData.InsertRange(Tk + 0x124, new byte[0x00 * 64]);
        }

        /// <summary>
        ///     Creates a blank ticket using the referenced title
        /// </summary>
        /// <param name="title">The title</param>
        /// <returns></returns>
        public static byte[] Create(Title title)
        {
            if (title == null)
                return null;

            var tiktem =
            ("00010004d15ea5ed15abe11ad15ea5ed15abe11ad15ea5ed15abe11ad15ea5ed15abe11a" +
             "d15ea5ed15abe11ad15ea5ed15abe11ad15ea5ed15abe11ad15ea5ed15abe11ad15ea5ed" +
             "15abe11ad15ea5ed15abe11ad15ea5ed15abe11ad15ea5ed15abe11ad15ea5ed15abe11a" +
             "d15ea5ed15abe11ad15ea5ed15abe11ad15ea5ed15abe11ad15ea5ed15abe11ad15ea5ed" +
             "15abe11ad15ea5ed15abe11ad15ea5ed15abe11ad15ea5ed15abe11ad15ea5ed15abe11a" +
             "d15ea5ed15abe11ad15ea5ed15abe11ad15ea5ed15abe11ad15ea5ed15abe11ad15ea5ed" +
             "15abe11ad15ea5ed15abe11ad15ea5ed15abe11ad15ea5ed15abe11ad15ea5ed15abe11a" +
             "d15ea5ed15abe11a00000000000000000000000000000000000000000000000000000000" +
             "0000000000000000000000000000000000000000000000000000000000000000526f6f74" +
             "2d434130303030303030332d585330303030303030630000000000000000000000000000" +
             "000000000000000000000000000000000000000000000000feedfacefeedfacefeedface" +
             "feedfacefeedfacefeedfacefeedfacefeedfacefeedfacefeedfacefeedfacefeedface" +
             "feedfacefeedfacefeedface010000cccccccccccccccccccccccccccccccc0000000000" +
             "0000000000000000aaaaaaaaaaaaaaaa0000000000000000000000000000000000000000" +
             "000000000000000000000000000000000000000000000000000000000000000000000000" +
             "000000000001000000000000000000000000000000000000000000000000000000000000" +
             "000000000000000000000000000000000000000000000000000000000000000000000000" +
             "000000000000000000000000000000000000000000000000000000000000000000000000" +
             "0000000000000000000000000000000000000000000000000000000000010014000000ac" +
             "000000140001001400000000000000280000000100000084000000840003000000000000" +
             "ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff00000000" +
             "000000000000000000000000000000000000000000000000000000000000000000000000" +
             "000000000000000000000000000000000000000000000000000000000000000000000000" +
             "0000000000000000000000000000000000000000").HexToBytes();

            var tikdata = new List<byte>(tiktem);

            switch (title.ContentType)
            {
                case "DLC":
                    PatchDlc(ref tikdata);
                    break;
                case "Demo":
                    PatchDemo(ref tikdata);
                    break;
            }

            tikdata.InsertRange(Tk + 0xA6, title.Versions[0].ToBytes());
            tikdata.InsertRange(Tk + 0x9C, title.ID.HexToBytes());
            tikdata.InsertRange(Tk + 0x7F, title.Key.HexToBytes());

            return tikdata.ToArray();
        }
    }
}