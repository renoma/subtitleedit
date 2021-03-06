using Nikse.SubtitleEdit.Core.Common;
using Nikse.SubtitleEdit.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Nikse.SubtitleEdit.Core.SubtitleFormats
{
    /// <summary>
    /// The PAC format was developed by Screen Electronics
    /// The PAC format save the contents, time code, position, justification, and italicization of each subtitle. The choice of font is not saved.
    /// </summary>
    public class Pac : SubtitleFormat, IBinaryPersistableSubtitle
    {
        public interface IGetPacEncoding
        {
            int GetPacEncoding(byte[] previewBuffer, string fileName);
        }

        public static IGetPacEncoding GetPacEncodingImplementation;

        public static readonly TimeCode PacNullTime = new TimeCode(655, 35, 00, 0);

        public static bool ThrowOnError = false;

        public static bool IsValidCodePage(int codePage)
        {
            return 0 <= codePage && codePage <= 11;
        }
        public const int CodePageLatin = 0;
        public const int CodePageGreek = 1;
        public const int CodePageLatinCzech = 2;
        public const int CodePageArabic = 3;
        public const int CodePageHebrew = 4;
        public const int CodePageThai = 5;
        public const int CodePageCyrillic = 6;
        public const int CodePageChineseTraditional = 7;
        public const int CodePageChineseSimplified = 8;
        public const int CodePageKorean = 9;
        public const int CodePageJapanese = 10;
        public const int CodePageLatinTurkish = 11;

        public const int EncodingChineseSimplified = 936;
        public const int EncodingChineseTraditional = 950;
        public const int EncodingKorean = 949;
        public const int EncodingJapanese = 932;

        /// <summary>
        /// Contains Swedish, Danish, German, Spanish, and French letters
        /// </summary>
        private static readonly Dictionary<int, SpecialCharacter> LatinCodes = new Dictionary<int, SpecialCharacter>
        {
            { 0xe041, new SpecialCharacter("??")},
            { 0xe04e, new SpecialCharacter("??")},
            { 0xe04f, new SpecialCharacter("??")},
            { 0xe061, new SpecialCharacter("??")},
            { 0xe06e, new SpecialCharacter("??")},
            { 0xe06f, new SpecialCharacter("??")},
            { 0xe161, new SpecialCharacter("??")},
            { 0xe141, new SpecialCharacter("??")},

            { 0x618a, new SpecialCharacter("??")},
            { 0x418a, new SpecialCharacter("??")},
            { 0x458a, new SpecialCharacter("??")},
            { 0x658a, new SpecialCharacter("??")},
            { 0x498a, new SpecialCharacter("??")},
            { 0x698a, new SpecialCharacter("??")},
            { 0x4f8a, new SpecialCharacter("??")},
            { 0x6f8a, new SpecialCharacter("??")},
            { 0x558a, new SpecialCharacter("??")},
            { 0x758a, new SpecialCharacter("??")},

            { 0x23, new SpecialCharacter("??")},
            { 0x7c, new SpecialCharacter("??")},
            { 0x7d, new SpecialCharacter("??")},
            { 0x7e, new SpecialCharacter("??")},
            { 0x80, new SpecialCharacter("#")},
            { 0x5c, new SpecialCharacter("??")},
            { 0x5d, new SpecialCharacter("??")},
            { 0x5e, new SpecialCharacter("??")},
            { 0x2d, new SpecialCharacter("-")},
            { 0x5f, new SpecialCharacter("???")},
            { 0xE54f, new SpecialCharacter("??")},
            { 0xE56f, new SpecialCharacter("??")},
            { 0xe541, new SpecialCharacter("??")},
            { 0xe561, new SpecialCharacter("??")},
            { 0xe555, new SpecialCharacter("??")},
            { 0xe575, new SpecialCharacter("??")},
            { 0x81,   new SpecialCharacter("??")},
            { 0x82,   new SpecialCharacter("??")},
            { 0xe241, new SpecialCharacter("??")},
            { 0xe249, new SpecialCharacter("??")},
            { 0xe255, new SpecialCharacter("??")},
            { 0xe259, new SpecialCharacter("??")},
            { 0xe261, new SpecialCharacter("??")},
            { 0xe265, new SpecialCharacter("??")},
            { 0xe269, new SpecialCharacter("??")},
            { 0xe245, new SpecialCharacter("??")},
            { 0xe275, new SpecialCharacter("??")}, // or "??" !?
            { 0xe279, new SpecialCharacter("??")},
            { 0xe361, new SpecialCharacter("??")},
            { 0xe365, new SpecialCharacter("??")},
            { 0xe36f, new SpecialCharacter("??")},
            { 0xe345, new SpecialCharacter("??")},
            { 0xe349, new SpecialCharacter("??")},
            { 0xe34f, new SpecialCharacter("??")},
            { 0xe369, new SpecialCharacter("??")},
            { 0xe443, new SpecialCharacter("??")},
            { 0xe447, new SpecialCharacter("??")},
            { 0xe448, new SpecialCharacter("??")},
            { 0xe44A, new SpecialCharacter("??")},
            { 0xe453, new SpecialCharacter("??")},
            { 0xeA55, new SpecialCharacter("??")},
            { 0xe463, new SpecialCharacter("??")},
            { 0xe467, new SpecialCharacter("??")},
            { 0xe468, new SpecialCharacter("??")},
            { 0xe46A, new SpecialCharacter("??")},
            { 0xe473, new SpecialCharacter("??")},
            { 0xeA75, new SpecialCharacter("??")},
            { 0xe341, new SpecialCharacter("??")},
            { 0xe441, new SpecialCharacter("??")},
            { 0xe461, new SpecialCharacter("??")},
            { 0xe643, new SpecialCharacter("??")},
            { 0xe663, new SpecialCharacter("??")},
            { 0xe445, new SpecialCharacter("??")},
            { 0xe465, new SpecialCharacter("??")},
            { 0xe545, new SpecialCharacter("??")},
            { 0xe565, new SpecialCharacter("??")},
            { 0xe449, new SpecialCharacter("??")},
            { 0xe469, new SpecialCharacter("??")},
            { 0xe549, new SpecialCharacter("??")},
            { 0xe569, new SpecialCharacter("??")},
            { 0xe44F, new SpecialCharacter("??")},
            { 0xe46F, new SpecialCharacter("??")},
            { 0xe355, new SpecialCharacter("??")},
            { 0xe375, new SpecialCharacter("??")},
            { 0xe455, new SpecialCharacter("??")},
            { 0xe475, new SpecialCharacter("??")},
            { 0xe559, new SpecialCharacter("??")},
            { 0xe579, new SpecialCharacter("??")},
            { 0xeb41, new SpecialCharacter("??")},
            { 0xeb61, new SpecialCharacter("??")},
            { 0xe243, new SpecialCharacter("??")},
            { 0xe263, new SpecialCharacter("??")},
            { 0xeB45, new SpecialCharacter("??")},
            { 0xeB65, new SpecialCharacter("??")},
            { 0x9c,   new SpecialCharacter("??")},
            { 0xbc,   new SpecialCharacter("??")},
            { 0xe24e, new SpecialCharacter("??")},
            { 0xe26e, new SpecialCharacter("??")},
            { 0xe24f, new SpecialCharacter("??")},
            { 0xe26f, new SpecialCharacter("??")},
            { 0xe253, new SpecialCharacter("??")},
            { 0xe273, new SpecialCharacter("??")},
            { 0xe25a, new SpecialCharacter("??")},
            { 0xe27a, new SpecialCharacter("??")},
            { 0xe85a, new SpecialCharacter("??")},
            { 0xe87a, new SpecialCharacter("??")},
            { 0x87, new SpecialCharacter("??")},
            { 0x89, new SpecialCharacter("??")},
            { 0x88, new SpecialCharacter("??")},
            { 0x8c, new SpecialCharacter("??")},

            { 0xe653, new SpecialCharacter("??")},
            { 0xe673, new SpecialCharacter("??")},
            { 0x7b,   new SpecialCharacter("??")},
            { 0xeA67, new SpecialCharacter("??")},
            { 0xeA47, new SpecialCharacter("??")},
            { 0xe849, new SpecialCharacter("??")},

            { 0xE75A, new SpecialCharacter("??")},
            { 0xE753, new SpecialCharacter("??")},
            { 0xE743, new SpecialCharacter("??")},

            { 0xE77A, new SpecialCharacter("??")},
            { 0xE773, new SpecialCharacter("??")},
            { 0xE763, new SpecialCharacter("??")},
            { 0xAE, new SpecialCharacter("??")},

            { 0xA8,  new SpecialCharacter("??")},
            { 0xAD,  new SpecialCharacter("??")},
            { 0xA6,  new SpecialCharacter("??")},
            { 0xA7,  new SpecialCharacter("??")},

            { 0xAB, new SpecialCharacter("??")},
            { 0xBB, new SpecialCharacter("??")},
            { 0xB3, new SpecialCharacter("??")},
            { 0x1C, new SpecialCharacter("???")},
            { 0x1D, new SpecialCharacter("???")},
            { 0x18, new SpecialCharacter("???")},
            { 0x19, new SpecialCharacter("???")},
            { 0x13, new SpecialCharacter("???")},
            { 0x14, new SpecialCharacter("???")},
            { 0x83, new SpecialCharacter("??")},
            { 0x5B, new SpecialCharacter("??")},
            { 0x85, new SpecialCharacter("??")},
            { 0x86, new SpecialCharacter("??")},
            { 0x8A, new SpecialCharacter("??")},
            { 0x8B, new SpecialCharacter("??")},
            { 0x8E, new SpecialCharacter("??")},
            { 0x8D, new SpecialCharacter("??")},
            { 0x95, new SpecialCharacter("??")},
            { 0x96, new SpecialCharacter("??")},
            { 0x9A, new SpecialCharacter("??")},
            { 0x9B, new SpecialCharacter("??")},
            { 0x9D, new SpecialCharacter("??")},
            { 0x9E, new SpecialCharacter("???")},
            { 0xA9, new SpecialCharacter("??")},
            { 0xAA, new SpecialCharacter("??")},
            { 0xAC, new SpecialCharacter("??")},
            { 0xB1, new SpecialCharacter("??")},
            { 0xB5, new SpecialCharacter("??")},
            { 0xBA, new SpecialCharacter("??")},
            { 0xBF, new SpecialCharacter("??")},
            { 0xD2, new SpecialCharacter("??")},
            { 0xD4, new SpecialCharacter("??")},
            { 0x4CE2, new SpecialCharacter("??", priority: 1)},
            { 0x74E2, new SpecialCharacter("??", priority: 1)},
            { 0x64E2, new SpecialCharacter("??", priority: 1)},
            { 0x6CE2, new SpecialCharacter("??", priority: 1)},
            { 0xE020, new SpecialCharacter("??", priority: 0)},
            { 0xE045, new SpecialCharacter("???")},
            { 0xE049, new SpecialCharacter("??")},
            { 0xE055, new SpecialCharacter("??")},
            { 0xE056, new SpecialCharacter("???")},
            { 0xE059, new SpecialCharacter("???")},
            { 0xE065, new SpecialCharacter("???")},
            { 0xE069, new SpecialCharacter("??")},
            { 0xE075, new SpecialCharacter("??")},
            { 0xE076, new SpecialCharacter("???")},
            { 0xE079, new SpecialCharacter("???")},
            { 0xE120, new SpecialCharacter("??", priority: 0)},
            { 0xE155, new SpecialCharacter("??")},
            { 0xE175, new SpecialCharacter("??")},
            { 0xE177, new SpecialCharacter("???")},
            { 0xE179, new SpecialCharacter("???")},
            { 0xE220, new SpecialCharacter("??", priority: 0)},
            { 0xE247, new SpecialCharacter("??")},
            { 0xE24B, new SpecialCharacter("???")},
            { 0xE24C, new SpecialCharacter("??")},
            { 0xE24D, new SpecialCharacter("???")},
            { 0xE250, new SpecialCharacter("???")},
            { 0xE252, new SpecialCharacter("??")},
            { 0xE257, new SpecialCharacter("???")},
            { 0xE25C, new SpecialCharacter("??")},
            { 0xE25D, new SpecialCharacter("??")},
            { 0xE267, new SpecialCharacter("??")},
            { 0xE26B, new SpecialCharacter("???")},
            { 0xE26C, new SpecialCharacter("??")},
            { 0xE26D, new SpecialCharacter("???")},
            { 0xE270, new SpecialCharacter("???")},
            { 0xE272, new SpecialCharacter("??")},
            { 0xE277, new SpecialCharacter("???")},
            { 0xE27C, new SpecialCharacter("??")},
            { 0xE27D, new SpecialCharacter("??")},
            { 0xE320, new SpecialCharacter("`", priority: 0)},
            { 0xE34E, new SpecialCharacter("??")},
            { 0xE357, new SpecialCharacter("???")},
            { 0xE359, new SpecialCharacter("???")},
            { 0xE36E, new SpecialCharacter("??")},
            { 0xE377, new SpecialCharacter("???")},
            { 0xE379, new SpecialCharacter("???")},
            { 0xE420, new SpecialCharacter("^", priority: 0)},
            { 0xE457, new SpecialCharacter("??")},
            { 0xE459, new SpecialCharacter("??")},
            { 0xE45A, new SpecialCharacter("???")},
            { 0xE477, new SpecialCharacter("??")},
            { 0xE479, new SpecialCharacter("??")},
            { 0xE47A, new SpecialCharacter("???")},
            { 0xE520, new SpecialCharacter("??", priority: 0)},
            { 0xE548, new SpecialCharacter("???")},
            { 0xE557, new SpecialCharacter("???")},
            { 0xE558, new SpecialCharacter("???")},
            { 0xE568, new SpecialCharacter("???")},
            { 0xE574, new SpecialCharacter("???")},
            { 0xE577, new SpecialCharacter("???")},
            { 0xE578, new SpecialCharacter("???")},
            { 0xE620, new SpecialCharacter("??", priority: 0)},
            { 0xE644, new SpecialCharacter("???")},
            { 0xE645, new SpecialCharacter("??")},
            { 0xE647, new SpecialCharacter("??")},
            { 0xE648, new SpecialCharacter("???")},
            { 0xE64B, new SpecialCharacter("??")},
            { 0xE64C, new SpecialCharacter("??")},
            { 0xE64E, new SpecialCharacter("??")},
            { 0xE652, new SpecialCharacter("??")},
            { 0xE654, new SpecialCharacter("??")},
            { 0xE664, new SpecialCharacter("???")},
            { 0xE665, new SpecialCharacter("??")},
            { 0xE667, new SpecialCharacter("??")},
            { 0xE668, new SpecialCharacter("???")},
            { 0xE66B, new SpecialCharacter("??")},
            { 0xE66C, new SpecialCharacter("??")},
            { 0xE66E, new SpecialCharacter("??")},
            { 0xE672, new SpecialCharacter("??")},
            { 0xE674, new SpecialCharacter("??")},
            { 0xE720, new SpecialCharacter("??", priority: 0)},
            { 0xE741, new SpecialCharacter("??")},
            { 0xE744, new SpecialCharacter("??")},
            { 0xE745, new SpecialCharacter("??")},
            { 0xE747, new SpecialCharacter("??")},
            { 0xE748, new SpecialCharacter("??")},
            { 0xE749, new SpecialCharacter("??")},
            { 0xE74B, new SpecialCharacter("??")},
            { 0xE74E, new SpecialCharacter("??")},
            { 0xE74F, new SpecialCharacter("??")},
            { 0xE752, new SpecialCharacter("??")},
            { 0xE754, new SpecialCharacter("??")},
            { 0xE755, new SpecialCharacter("??")},
            { 0xE761, new SpecialCharacter("??")},
            { 0xE765, new SpecialCharacter("??")},
            { 0xE768, new SpecialCharacter("??")},
            { 0xE769, new SpecialCharacter("??")},
            { 0xE76A, new SpecialCharacter("??")},
            { 0xE76B, new SpecialCharacter("??")},
            { 0xE76E, new SpecialCharacter("??")},
            { 0xE76F, new SpecialCharacter("??")},
            { 0xE772, new SpecialCharacter("??")},
            { 0xE775, new SpecialCharacter("??")},
            { 0xE820, new SpecialCharacter("??", priority: 0)},
            { 0xE841, new SpecialCharacter("??")},
            { 0xE842, new SpecialCharacter("???")},
            { 0xE843, new SpecialCharacter("??")},
            { 0xE844, new SpecialCharacter("???")},
            { 0xE845, new SpecialCharacter("??")},
            { 0xE846, new SpecialCharacter("???")},
            { 0xE847, new SpecialCharacter("??")},
            { 0xE848, new SpecialCharacter("???")},
            { 0xE84D, new SpecialCharacter("???")},
            { 0xE84E, new SpecialCharacter("???")},
            { 0xE84F, new SpecialCharacter("??")},
            { 0xE850, new SpecialCharacter("???")},
            { 0xE852, new SpecialCharacter("???")},
            { 0xE853, new SpecialCharacter("???")},
            { 0xE854, new SpecialCharacter("???")},
            { 0xE857, new SpecialCharacter("???")},
            { 0xE858, new SpecialCharacter("???")},
            { 0xE859, new SpecialCharacter("???")},
            { 0xE861, new SpecialCharacter("??")},
            { 0xE862, new SpecialCharacter("???")},
            { 0xE863, new SpecialCharacter("??")},
            { 0xE864, new SpecialCharacter("???")},
            { 0xE865, new SpecialCharacter("??")},
            { 0xE866, new SpecialCharacter("???")},
            { 0xE867, new SpecialCharacter("??")},
            { 0xE868, new SpecialCharacter("???")},
            { 0xE86D, new SpecialCharacter("???")},
            { 0xE86E, new SpecialCharacter("???")},
            { 0xE86F, new SpecialCharacter("??")},
            { 0xE870, new SpecialCharacter("???")},
            { 0xE872, new SpecialCharacter("???")},
            { 0xE873, new SpecialCharacter("???")},
            { 0xE874, new SpecialCharacter("???")},
            { 0xE877, new SpecialCharacter("???")},
            { 0xE878, new SpecialCharacter("???")},
            { 0xE879, new SpecialCharacter("???")},
            { 0xE920, new SpecialCharacter("??", priority: 0)},
            { 0xE941, new SpecialCharacter("??")},
            { 0xE945, new SpecialCharacter("??")},
            { 0xE947, new SpecialCharacter("???")},
            { 0xE949, new SpecialCharacter("??")},
            { 0xE94F, new SpecialCharacter("??")},
            { 0xE955, new SpecialCharacter("??")},
            { 0xE959, new SpecialCharacter("??")},
            { 0xE95C, new SpecialCharacter("??")},
            { 0xE961, new SpecialCharacter("??")},
            { 0xE965, new SpecialCharacter("??")},
            { 0xE967, new SpecialCharacter("???")},
            { 0xE969, new SpecialCharacter("??")},
            { 0xE96F, new SpecialCharacter("??")},
            { 0xE975, new SpecialCharacter("??")},
            { 0xE979, new SpecialCharacter("??")},
            { 0xE97C, new SpecialCharacter("??")},
            { 0xEA20, new SpecialCharacter("??", priority: 0)},
            { 0xEA41, new SpecialCharacter("??")},
            { 0xEA45, new SpecialCharacter("??")},
            { 0xEA49, new SpecialCharacter("??")},
            { 0xEA4F, new SpecialCharacter("??")},
            { 0xEA61, new SpecialCharacter("??")},
            { 0xEA65, new SpecialCharacter("??")},
            { 0xEA69, new SpecialCharacter("??")},
            { 0xEA6F, new SpecialCharacter("??")},
            { 0xEB20, new SpecialCharacter("??", priority: 0)},
            { 0xEB49, new SpecialCharacter("??")},
            { 0xEB4F, new SpecialCharacter("??")},
            { 0xEB55, new SpecialCharacter("??")},
            { 0xEB69, new SpecialCharacter("??")},
            { 0xEB6F, new SpecialCharacter("??")},
            { 0xEB75, new SpecialCharacter("??")},
            { 0xEC20, new SpecialCharacter("??", priority: 0)},
            { 0xEC4F, new SpecialCharacter("??")},
            { 0xEC55, new SpecialCharacter("??")},
            { 0xEC6F, new SpecialCharacter("??")},
            { 0xEC75, new SpecialCharacter("??")},
            { 0xC0, new SpecialCharacter("[")},
            { 0xC1, new SpecialCharacter("]")},
        };

        private static readonly Dictionary<int, SpecialCharacter> LatinTurkishOverrides = new Dictionary<int, SpecialCharacter>
        {
            { 0xE720, new SpecialCharacter("??", priority: 0) },
            { 0xE741, new SpecialCharacter("??")},
            { 0xE745, new SpecialCharacter("??")},
            { 0xE747, new SpecialCharacter("??")},
            { 0xE749, new SpecialCharacter("??")},
            { 0xE74F, new SpecialCharacter("??")},
            { 0xE755, new SpecialCharacter("??")},
            { 0xE761, new SpecialCharacter("??")},
            { 0xE765, new SpecialCharacter("??")},
            { 0xE769, new SpecialCharacter("??")},
            { 0xE76F, new SpecialCharacter("??")},
            { 0xE775, new SpecialCharacter("??")},

            { 0xEA20, new SpecialCharacter("??", priority: 0) },
            { 0xEA45, new SpecialCharacter("??")},
            { 0xEA47, new SpecialCharacter("??")},
            { 0xEA49, new SpecialCharacter("??")},
            { 0xEA4F, new SpecialCharacter("??")},
            { 0xEA55, new SpecialCharacter("??")},
            { 0xEA65, new SpecialCharacter("??")},
            { 0xEA67, new SpecialCharacter("??")},
            { 0xEA69, new SpecialCharacter("??")},
            { 0xEA6F, new SpecialCharacter("??")},
            { 0xEA75, new SpecialCharacter("??")},
        };

        private static readonly Dictionary<int, SpecialCharacter> HebrewCodes = new Dictionary<int, SpecialCharacter>
        {
            { 0x80, new SpecialCharacter("??")},
            { 0x81, new SpecialCharacter("??")},
            { 0x82, new SpecialCharacter("??")},
            { 0x83, new SpecialCharacter("??")},
            { 0x84, new SpecialCharacter("??")},
            { 0x85, new SpecialCharacter("??")},
            { 0x86, new SpecialCharacter("??")},
            { 0x87, new SpecialCharacter("??")},
            { 0x88, new SpecialCharacter("??")},
            { 0x89, new SpecialCharacter("??")},
            { 0x8b, new SpecialCharacter("??")},
            { 0x8c, new SpecialCharacter("??")},
            { 0x8d, new SpecialCharacter("??")},
            { 0x92, new SpecialCharacter("??")},
            { 0xa0, new SpecialCharacter("??")},
            { 0xa1, new SpecialCharacter("??")},
            { 0xa2, new SpecialCharacter("??")},
            { 0xa3, new SpecialCharacter("??")},
            { 0xa4, new SpecialCharacter("??")},
            { 0xa5, new SpecialCharacter("??")},
            { 0xa6, new SpecialCharacter("??")},
            { 0xa7, new SpecialCharacter("??")},
            { 0xa8, new SpecialCharacter("??")},
            { 0xa9, new SpecialCharacter("??")},
            { 0xaa, new SpecialCharacter("??")},
            { 0xab, new SpecialCharacter("??")},
            { 0xac, new SpecialCharacter("??")},
            { 0xad, new SpecialCharacter("??")},
            { 0xae, new SpecialCharacter("??")},
            { 0xaf, new SpecialCharacter("??")},
            { 0xb0, new SpecialCharacter("??")},
            { 0xb1, new SpecialCharacter("??")},
            { 0xb2, new SpecialCharacter("??")},
            { 0xb3, new SpecialCharacter("??")},
            { 0xb4, new SpecialCharacter("??")},
            { 0xb5, new SpecialCharacter("??")},
            { 0xb6, new SpecialCharacter("??")},
            { 0xb7, new SpecialCharacter("??")},
            { 0xb8, new SpecialCharacter("??")},
            { 0xb9, new SpecialCharacter("??")},
            { 0xba, new SpecialCharacter("??")},
            { 0xbb, new SpecialCharacter("??")},
            { 0xbc, new SpecialCharacter("??")},
            { 0xbd, new SpecialCharacter("??")},
            { 0xcc, new SpecialCharacter("???")},
            { 0xcd, new SpecialCharacter("???")},
            { 0xce, new SpecialCharacter("???")},
            { 0xcf, new SpecialCharacter("???")},
            { 0xd0, new SpecialCharacter("???")},
            { 0xd1, new SpecialCharacter("???")},
            { 0xd2, new SpecialCharacter("???")},
            { 0xd3, new SpecialCharacter("???")},
            { 0xd4, new SpecialCharacter("???")},
            { 0xd5, new SpecialCharacter("???")},
            { 0xd6, new SpecialCharacter("???")},
            { 0xd7, new SpecialCharacter("???")},
            { 0xd8, new SpecialCharacter("???")},
            { 0xd9, new SpecialCharacter("???")},
            { 0xda, new SpecialCharacter("???")},
            { 0xdb, new SpecialCharacter("???")},
            { 0xdc, new SpecialCharacter("???")},
            { 0xdd, new SpecialCharacter("???")},
            { 0xde, new SpecialCharacter("???")},
            { 0xdf, new SpecialCharacter("???")},
            { 0x2e, new SpecialCharacter(".")},
            { 0x2c, new SpecialCharacter(",")}
        };

        private static readonly Dictionary<int, SpecialCharacter> ArabicCodes = new Dictionary<int, SpecialCharacter>
        {
            { 0xe081, new SpecialCharacter("??")},
            { 0xe086, new SpecialCharacter("??")},
            { 0xe09b, new SpecialCharacter("??")},
            { 0xe09c, new SpecialCharacter("??")},
            { 0xe181, new SpecialCharacter("??")},
            { 0xe281, new SpecialCharacter("??")},
            { 0xe781, new SpecialCharacter("????")},
            { 0x80, new SpecialCharacter("??")},
            { 0x81, new SpecialCharacter("??")},
            { 0x82, new SpecialCharacter("??")},
            { 0x83, new SpecialCharacter("??")},
            { 0x84, new SpecialCharacter("??")},
            { 0x85, new SpecialCharacter("??")},
            { 0x86, new SpecialCharacter("??")},
            { 0x87, new SpecialCharacter("??")},
            { 0x88, new SpecialCharacter("??")},
            { 0x89, new SpecialCharacter("??")},
            { 0x8A, new SpecialCharacter("??")},
            { 0x8b, new SpecialCharacter("??")},
            { 0x8c, new SpecialCharacter("??")},
            { 0x8d, new SpecialCharacter("??")},
            { 0x8e, new SpecialCharacter("??")},
            { 0x8f, new SpecialCharacter("??")},
            { 0x90, new SpecialCharacter("??")},
            { 0x91, new SpecialCharacter("??")},
            { 0x92, new SpecialCharacter("??")},
            { 0x93, new SpecialCharacter("??")},
            { 0x94, new SpecialCharacter("??")},
            { 0x95, new SpecialCharacter("??")},
            { 0x96, new SpecialCharacter("??")},
            { 0x97, new SpecialCharacter("??")},
            { 0x98, new SpecialCharacter("??")},
            { 0x99, new SpecialCharacter("??")},
            { 0x9A, new SpecialCharacter("??")},
            { 0x9b, new SpecialCharacter("??")},
            { 0x9c, new SpecialCharacter("??")},
            { 0x9d, new SpecialCharacter("??")},
            { 0x9e, new SpecialCharacter("????")},
            { 0x9f, new SpecialCharacter("??")},
            { 0xe09f, new SpecialCharacter("??")},
            { 0xa0, new SpecialCharacter("??")},
            { 0xad, new SpecialCharacter("??")},
            { 0xae, new SpecialCharacter("???")},
            { 0xb0, new SpecialCharacter("???")},
            { 0xb1, new SpecialCharacter("???")},
            { 0xb3, new SpecialCharacter("??")},
            { 0x3f, new SpecialCharacter("??")},
            { 0x25, new SpecialCharacter("??")},
            { 0x2c, new SpecialCharacter("??")},
            { 0x3b, new SpecialCharacter("??")},
            { 0xe7, new SpecialCharacter("\u064B", true)},
            { 0xea, new SpecialCharacter("\u064C", true)},
            { 0xe8, new SpecialCharacter("\u064D", true)},
            { 0xe5, new SpecialCharacter("\u064E", true)},
            { 0xe4, new SpecialCharacter("\u064F", true)},
            { 0xe6, new SpecialCharacter("\u0650", true)},
            { 0xe3, new SpecialCharacter("\u0651", true)},
            { 0xe9, new SpecialCharacter("\u0652", true)},
            { 0xe2, new SpecialCharacter("\u0653", true)},
            { 0xe0, new SpecialCharacter("\u0654", true)},
            { 0xe1, new SpecialCharacter("\u0655", true)},
        };

        private static readonly Dictionary<int, SpecialCharacter> CyrillicCodes = new Dictionary<int, SpecialCharacter>
        {
            { 0x20, new SpecialCharacter(" ")},
            { 0x21, new SpecialCharacter("!")},
            { 0x22, new SpecialCharacter("??")},
            { 0x23, new SpecialCharacter("/")},
            { 0x24, new SpecialCharacter("?")},
            { 0x25, new SpecialCharacter(":")},
            { 0x26, new SpecialCharacter(".")},
            { 0x27, new SpecialCharacter("??")},
            { 0x28, new SpecialCharacter("(")},
            { 0x29, new SpecialCharacter(")")},
            { 0x2a, new SpecialCharacter(";")},
            { 0x2b, new SpecialCharacter("+")},
            { 0x2c, new SpecialCharacter("??")},
            { 0x2d, new SpecialCharacter("-")},
            { 0x2e, new SpecialCharacter("??")},
            { 0x3a, new SpecialCharacter("??")},
            { 0x3b, new SpecialCharacter("??")},
            { 0x3c, new SpecialCharacter("<")},
            { 0x3d, new SpecialCharacter("=")},
            { 0x3e, new SpecialCharacter(">")},
            { 0x41, new SpecialCharacter("??")},
            { 0x42, new SpecialCharacter("??")},
            { 0x43, new SpecialCharacter("??")},
            { 0x44, new SpecialCharacter("??")},
            { 0x45, new SpecialCharacter("??")},
            { 0x46, new SpecialCharacter("??")},
            { 0x47, new SpecialCharacter("??")},
            { 0x48, new SpecialCharacter("??")},
            { 0x49, new SpecialCharacter("??")},
            { 0x4a, new SpecialCharacter("??")},
            { 0x4b, new SpecialCharacter("??")},
            { 0x4c, new SpecialCharacter("??")},
            { 0x4d, new SpecialCharacter("??")},
            { 0x4e, new SpecialCharacter("??")},
            { 0x4f, new SpecialCharacter("??")},
            { 0x50, new SpecialCharacter("??")},
            { 0x51, new SpecialCharacter("Q")},
            { 0x52, new SpecialCharacter("??")},
            { 0x53, new SpecialCharacter("??")},
            { 0x54, new SpecialCharacter("??")},
            { 0x55, new SpecialCharacter("??")},
            { 0x56, new SpecialCharacter("??")},
            { 0x57, new SpecialCharacter("??")},
            { 0x58, new SpecialCharacter("??")},
            { 0x59, new SpecialCharacter("??")},
            { 0x5a, new SpecialCharacter("??")},
            { 0x5b, new SpecialCharacter("??")},
            { 0x5d, new SpecialCharacter("??")},
            { 0x5e, new SpecialCharacter(",")},
            { 0x5f, new SpecialCharacter("-")},
            { 0x61, new SpecialCharacter("??")},
            { 0x62, new SpecialCharacter("??")},
            { 0x63, new SpecialCharacter("??")},
            { 0x64, new SpecialCharacter("??")},
            { 0x65, new SpecialCharacter("??")},
            { 0x66, new SpecialCharacter("??")},
            { 0x67, new SpecialCharacter("??")},
            { 0x68, new SpecialCharacter("??")},
            { 0x69, new SpecialCharacter("??")},
            { 0x6a, new SpecialCharacter("??")},
            { 0x6b, new SpecialCharacter("??")},
            { 0x6c, new SpecialCharacter("??")},
            { 0x6d, new SpecialCharacter("??")},
            { 0x6e, new SpecialCharacter("??")},
            { 0x6f, new SpecialCharacter("??")},
            { 0x70, new SpecialCharacter("??")},
            { 0x72, new SpecialCharacter("??")},
            { 0x73, new SpecialCharacter("??")},
            { 0x74, new SpecialCharacter("??")},
            { 0x75, new SpecialCharacter("??")},
            { 0x76, new SpecialCharacter("??")},
            { 0x77, new SpecialCharacter("??")},
            { 0x78, new SpecialCharacter("??")},
            { 0x79, new SpecialCharacter("??")},
            { 0x7a, new SpecialCharacter("??")},
            { 0x7b, new SpecialCharacter("??")},
            { 0x7d, new SpecialCharacter("??")},
            { 0x80, new SpecialCharacter("??")},
            { 0x81, new SpecialCharacter("??")},
            { 0x82, new SpecialCharacter("??")},
            { 0x84, new SpecialCharacter("??")},
            { 0x85, new SpecialCharacter("??")},
            { 0x86, new SpecialCharacter("??")},
            { 0x88, new SpecialCharacter("??")},
            { 0x89, new SpecialCharacter("??")},
            { 0x8a, new SpecialCharacter("??")},
            { 0x8b, new SpecialCharacter("??")},
            { 0x8c, new SpecialCharacter("??")},
            { 0x8d, new SpecialCharacter("??")},
            { 0x8f, new SpecialCharacter("??")},
            { 0x90, new SpecialCharacter("???")},
            { 0x92, new SpecialCharacter("??")},
            { 0x94, new SpecialCharacter("??")},
            { 0x95, new SpecialCharacter("??")},
            { 0x96, new SpecialCharacter("??")},
            { 0x98, new SpecialCharacter("??")},
            { 0x99, new SpecialCharacter("??")},
            { 0x9a, new SpecialCharacter("??")},
            { 0x9b, new SpecialCharacter("??")},
            { 0x9d, new SpecialCharacter("??")},
            { 0x9f, new SpecialCharacter("??")},
            { 0xa2, new SpecialCharacter("%")},
            { 0xa4, new SpecialCharacter("&")},
            { 0xac, new SpecialCharacter("C")},
            { 0xad, new SpecialCharacter("D")},
            { 0xae, new SpecialCharacter("E")},
            { 0xaf, new SpecialCharacter("F")},
            { 0xb0, new SpecialCharacter("G")},
            { 0xb1, new SpecialCharacter("H")},
            { 0xb2, new SpecialCharacter("'")},
            { 0xb3, new SpecialCharacter("\"")},
            { 0xb4, new SpecialCharacter("I")},
            { 0xb5, new SpecialCharacter("J")},
            { 0xb6, new SpecialCharacter("K")},
            { 0xb7, new SpecialCharacter("L")},
            { 0xb8, new SpecialCharacter("M")},
            { 0xb9, new SpecialCharacter("N")},
            { 0xba, new SpecialCharacter("P")},
            { 0xbb, new SpecialCharacter("Q")},
            { 0xbc, new SpecialCharacter("R")},
            { 0xbd, new SpecialCharacter("S")},
            { 0xbe, new SpecialCharacter("T")},
            { 0xbf, new SpecialCharacter("U")},
            { 0xc0, new SpecialCharacter("V")},
            { 0xc1, new SpecialCharacter("*")},
            { 0xc2, new SpecialCharacter("W")},
            { 0xc3, new SpecialCharacter("X")},
            { 0xc4, new SpecialCharacter("Y")},
            { 0xc5, new SpecialCharacter("Z")},
            { 0xc6, new SpecialCharacter("b")},
            { 0xc7, new SpecialCharacter("c")},
            { 0xc8, new SpecialCharacter("d")},
            { 0xc9, new SpecialCharacter("e")},
            { 0xca, new SpecialCharacter("f")},
            { 0xcb, new SpecialCharacter("g")},
            { 0xcc, new SpecialCharacter("h")},
            { 0xcd, new SpecialCharacter("i")},
            { 0xce, new SpecialCharacter("j")},
            { 0xcf, new SpecialCharacter("k")},
            { 0xd0, new SpecialCharacter("???")},
            { 0xd1, new SpecialCharacter("l")},
            { 0xd2, new SpecialCharacter("m")},
            { 0xd3, new SpecialCharacter("n")},
            { 0xd4, new SpecialCharacter("o")},
            { 0xd5, new SpecialCharacter("p")},
            { 0xd6, new SpecialCharacter("q")},
            { 0xd7, new SpecialCharacter("r")},
            { 0xd8, new SpecialCharacter("s")},
            { 0xd9, new SpecialCharacter("t")},
            { 0xda, new SpecialCharacter("u")},
            { 0xdb, new SpecialCharacter("v")},
            { 0xdc, new SpecialCharacter("w")},
            { 0xdd, new SpecialCharacter("??")},
            { 0xde, new SpecialCharacter("??")},
            { 0xdf, new SpecialCharacter("z")},
            { 0xe3, new SpecialCharacter("`")},
            { 0xe5, new SpecialCharacter("??")},
            { 0xe020, new SpecialCharacter("??")},
            { 0xe03a, new SpecialCharacter("??")},
            { 0xe03b, new SpecialCharacter("??")},
            { 0xe042, new SpecialCharacter("??")},
            { 0xe045, new SpecialCharacter("??")},
            { 0xe046, new SpecialCharacter("??")},
            { 0xe054, new SpecialCharacter("??")},
            { 0xe062, new SpecialCharacter("??")},
            { 0xe065, new SpecialCharacter("??")},
            { 0xe066, new SpecialCharacter("??")},
            { 0xe074, new SpecialCharacter("??")},
            { 0xe220, new SpecialCharacter("??")},
            { 0xe252, new SpecialCharacter("??")},
            { 0xe255, new SpecialCharacter("??")},
            { 0xe272, new SpecialCharacter("??")},
            { 0xe275, new SpecialCharacter("??")},
            { 0xe342, new SpecialCharacter("??")},
            { 0xe354, new SpecialCharacter("??")},
            { 0xe362, new SpecialCharacter("??")},
            { 0xe374, new SpecialCharacter("??")},
            { 0xe522, new SpecialCharacter("??")},
            { 0xe527, new SpecialCharacter("??")},
            { 0xe53a, new SpecialCharacter("??")},
            { 0xe53b, new SpecialCharacter("??")},
            { 0xe542, new SpecialCharacter("??")},
            { 0xe545, new SpecialCharacter("??")},
            { 0xe546, new SpecialCharacter("??")},
            { 0xe54a, new SpecialCharacter("??")},
            { 0xe550, new SpecialCharacter("??")},
            { 0xe553, new SpecialCharacter("??")},
            { 0xe554, new SpecialCharacter("??")},
            { 0xe558, new SpecialCharacter("??")},
            { 0xe562, new SpecialCharacter("??")},
            { 0xe565, new SpecialCharacter("??")},
            { 0xe566, new SpecialCharacter("??")},
            { 0xe56a, new SpecialCharacter("??")},
            { 0xe570, new SpecialCharacter("??")},
            { 0xe573, new SpecialCharacter("??")},
            { 0xe574, new SpecialCharacter("??")},
            { 0xe578, new SpecialCharacter("??")},
            { 0xe586, new SpecialCharacter("??")},
            { 0xe596, new SpecialCharacter("??")},
            { 0xe5b4, new SpecialCharacter("??")},
        };

        private static readonly Dictionary<int, SpecialCharacter> KoreanCodes = new Dictionary<int, SpecialCharacter>
        {
            { 0x20, new SpecialCharacter(" ")}
        };

        private static readonly Dictionary<int, SpecialCharacter> GreekCodes = new Dictionary<int, SpecialCharacter>
        {
            { 0x20, new SpecialCharacter(" ") },
            { 0x21, new SpecialCharacter("!") },
            { 0x22, new SpecialCharacter("\"") },
            { 0x23, new SpecialCharacter("??") },
            { 0x24, new SpecialCharacter("$") },
            { 0x25, new SpecialCharacter("%") },
            { 0x26, new SpecialCharacter("&") },
            { 0x27, new SpecialCharacter("'") },
            { 0x28, new SpecialCharacter("(") },
            { 0x29, new SpecialCharacter(")") },
            { 0x2A, new SpecialCharacter("*") },
            { 0x2B, new SpecialCharacter("+") },
            { 0x2C, new SpecialCharacter(",") },
            { 0x2D, new SpecialCharacter("-") },
            { 0x2E, new SpecialCharacter(".") },
            { 0x2F, new SpecialCharacter("/") },
            { 0x3A, new SpecialCharacter(":") },
            { 0x3B, new SpecialCharacter(";") },
            { 0x3C, new SpecialCharacter("<") },
            { 0x3D, new SpecialCharacter("=") },
            { 0x3E, new SpecialCharacter(">") },
            { 0x3F, new SpecialCharacter("?") },
            { 0x40, new SpecialCharacter("@") },
            { 0x41, new SpecialCharacter("??") },
            { 0x42, new SpecialCharacter("??") },
            { 0x43, new SpecialCharacter("??") },
            { 0x44, new SpecialCharacter("??") },
            { 0x45, new SpecialCharacter("??") },
            { 0x46, new SpecialCharacter("??") },
            { 0x47, new SpecialCharacter("??") },
            { 0x48, new SpecialCharacter("??") },
            { 0x49, new SpecialCharacter("??") },
            { 0x4A, new SpecialCharacter("??") },
            { 0x4B, new SpecialCharacter("??") },
            { 0x4C, new SpecialCharacter("??") },
            { 0x4D, new SpecialCharacter("??") },
            { 0x4E, new SpecialCharacter("??") },
            { 0x4F, new SpecialCharacter("??") },
            { 0x50, new SpecialCharacter("??") },
            { 0x51, new SpecialCharacter("??") },
            { 0x52, new SpecialCharacter("R") },
            { 0x53, new SpecialCharacter("??") },
            { 0x54, new SpecialCharacter("??") },
            { 0x55, new SpecialCharacter("??") },
            { 0x56, new SpecialCharacter("??") },
            { 0x57, new SpecialCharacter("??") },
            { 0x58, new SpecialCharacter("??") },
            { 0x59, new SpecialCharacter("??") },
            { 0x5A, new SpecialCharacter("Z") },

            { 0x5F, new SpecialCharacter("-") },

            { 0x61, new SpecialCharacter("??") },
            { 0x62, new SpecialCharacter("??") },
            { 0x63, new SpecialCharacter("??") },
            { 0x64, new SpecialCharacter("??") },
            { 0x65, new SpecialCharacter("??") },
            { 0x66, new SpecialCharacter("??") },
            { 0x67, new SpecialCharacter("??") },
            { 0x68, new SpecialCharacter("??") },
            { 0x69, new SpecialCharacter("??") },
            { 0x6A, new SpecialCharacter("??") },
            { 0x6B, new SpecialCharacter("??") },
            { 0x6C, new SpecialCharacter("??") },
            { 0x6D, new SpecialCharacter("??") },
            { 0x6E, new SpecialCharacter("??") },
            { 0x6F, new SpecialCharacter("??") },
            { 0x70, new SpecialCharacter("??") },
            { 0x71, new SpecialCharacter("??") },
            { 0x72, new SpecialCharacter("??") },
            { 0x73, new SpecialCharacter("??") },
            { 0x74, new SpecialCharacter("??") },
            { 0x75, new SpecialCharacter("??") },
            { 0x76, new SpecialCharacter("??") },
            { 0x77, new SpecialCharacter("??") },
            { 0x78, new SpecialCharacter("??") },
            { 0x79, new SpecialCharacter("??") },
            { 0x7A, new SpecialCharacter("z") },
            { 0x7E, new SpecialCharacter("??") },
            { 0x80, new SpecialCharacter("#") },
            { 0x81, new SpecialCharacter("??") },
            { 0x82, new SpecialCharacter("??") },
            { 0x83, new SpecialCharacter("??") },
            { 0x84, new SpecialCharacter("??") },
            { 0x85, new SpecialCharacter("??") },
            { 0x86, new SpecialCharacter("??") },
            { 0x87, new SpecialCharacter("??") },
            { 0x88, new SpecialCharacter("??") },
            { 0x89, new SpecialCharacter("??") },
            { 0x8C, new SpecialCharacter("A") },
            { 0x8D, new SpecialCharacter("B") },
            { 0x8E, new SpecialCharacter("C") },
            { 0x8F, new SpecialCharacter("D") },
            { 0x90, new SpecialCharacter("E") },
            { 0x91, new SpecialCharacter("F") },
            { 0x92, new SpecialCharacter("G") },
            { 0x93, new SpecialCharacter("H") },
            { 0x94, new SpecialCharacter("I") },
            { 0x95, new SpecialCharacter("J") },
            { 0x96, new SpecialCharacter("K") },
            { 0x97, new SpecialCharacter("L") },
            { 0x98, new SpecialCharacter("M") },
            { 0x99, new SpecialCharacter("N") },
            { 0x9A, new SpecialCharacter("O") },
            { 0x9B, new SpecialCharacter("P") },
            { 0x9C, new SpecialCharacter("Q") },
            { 0x9D, new SpecialCharacter("R") },
            { 0x9E, new SpecialCharacter("S") },
            { 0x9F, new SpecialCharacter("T") },
            { 0xA0, new SpecialCharacter("U") },
            { 0xA1, new SpecialCharacter("V") },
            { 0xA2, new SpecialCharacter("W") },
            { 0xA3, new SpecialCharacter("X") },
            { 0xA4, new SpecialCharacter("Y") },
            { 0xA5, new SpecialCharacter("Z") },
            { 0xA9, new SpecialCharacter("??") },
            { 0xAB, new SpecialCharacter("??") },
            { 0xAC, new SpecialCharacter("a") },
            { 0xAD, new SpecialCharacter("b") },
            { 0xAE, new SpecialCharacter("c") },
            { 0xAF, new SpecialCharacter("d") },
            { 0xB0, new SpecialCharacter("e") },
            { 0xB1, new SpecialCharacter("f") },
            { 0xB2, new SpecialCharacter("g") },
            { 0xB3, new SpecialCharacter("h") },
            { 0xB4, new SpecialCharacter("i") },
            { 0xB5, new SpecialCharacter("j") },
            { 0xB6, new SpecialCharacter("k") },
            { 0xB7, new SpecialCharacter("l") },
            { 0xB8, new SpecialCharacter("m") },
            { 0xB9, new SpecialCharacter("n") },
            { 0xBA, new SpecialCharacter("o") },
            { 0xBB, new SpecialCharacter("p") },
            { 0xBC, new SpecialCharacter("q") },
            { 0xBD, new SpecialCharacter("r") },
            { 0xBE, new SpecialCharacter("s") },
            { 0xBF, new SpecialCharacter("t") },
            { 0xC0, new SpecialCharacter("u") },
            { 0xC1, new SpecialCharacter("v") },
            { 0xC2, new SpecialCharacter("w") },
            { 0xC3, new SpecialCharacter("x") },
            { 0xC4, new SpecialCharacter("y") },
            { 0xC5, new SpecialCharacter("z") },

            { 0x202A, new SpecialCharacter("??") },
            { 0xE241, new SpecialCharacter("??") },
            { 0xE242, new SpecialCharacter("????") },
            { 0xE243, new SpecialCharacter("????") },
            { 0xE244, new SpecialCharacter("????") },
            { 0xE245, new SpecialCharacter("??") },
            { 0xE246, new SpecialCharacter("????") },
            { 0xE247, new SpecialCharacter("??") },
            { 0xE248, new SpecialCharacter("????") },
            { 0xE249, new SpecialCharacter("??") },
            { 0xE24A, new SpecialCharacter("????") },
            { 0xE24B, new SpecialCharacter("????") },
            { 0xE24C, new SpecialCharacter("????") },
            { 0xE24D, new SpecialCharacter("????") },
            { 0xE24E, new SpecialCharacter("????") },
            { 0xE24F, new SpecialCharacter("??") },
            { 0xE255, new SpecialCharacter("??") },
            { 0xE258, new SpecialCharacter("????") },
            { 0xE259, new SpecialCharacter("??") },
            { 0xE261, new SpecialCharacter("??") },
            { 0xE262, new SpecialCharacter("????") },
            { 0xE263, new SpecialCharacter("????") },
            { 0xE264, new SpecialCharacter("????") },
            { 0xE265, new SpecialCharacter("??") },
            { 0xE266, new SpecialCharacter("??") },
            { 0xE267, new SpecialCharacter("??") },
            { 0xE268, new SpecialCharacter("????") },
            { 0xE269, new SpecialCharacter("??") },
            { 0xE26A, new SpecialCharacter("????") },
            { 0xE26B, new SpecialCharacter("????") },
            { 0xE26C, new SpecialCharacter("????") },
            { 0xE26D, new SpecialCharacter("????") },
            { 0xE26E, new SpecialCharacter("??") },
            { 0xE26F, new SpecialCharacter("??") },
            { 0xE270, new SpecialCharacter("??") },
            { 0xE271, new SpecialCharacter("????") },
            { 0xE272, new SpecialCharacter("????") },
            { 0xE273, new SpecialCharacter("????") },
            { 0xE274, new SpecialCharacter("????") },
            { 0xE275, new SpecialCharacter("??") },
            { 0xE276, new SpecialCharacter("????") },
            { 0xE277, new SpecialCharacter("????") },
            { 0xE278, new SpecialCharacter("????") },
            { 0xE279, new SpecialCharacter("??") },
            { 0xE27B, new SpecialCharacter("??") },
            { 0xE320, new SpecialCharacter("`") },
            { 0xE399, new SpecialCharacter("??") },
            { 0xE39A, new SpecialCharacter("??") },
            { 0xE3A0, new SpecialCharacter("??") },
            { 0xE3A2, new SpecialCharacter("???") },
            { 0xE3A4, new SpecialCharacter("???") },
            { 0xE3B9, new SpecialCharacter("??") },
            { 0xE3C2, new SpecialCharacter("???") },
            { 0xE3C4, new SpecialCharacter("???") },
            { 0xE549, new SpecialCharacter("??") },
            { 0xE555, new SpecialCharacter("??") },
            { 0xE561, new SpecialCharacter("????") },
            { 0xE562, new SpecialCharacter("????") },
            { 0xE563, new SpecialCharacter("????") },
            { 0xE564, new SpecialCharacter("????") },
            { 0xE565, new SpecialCharacter("????") },
            { 0xE566, new SpecialCharacter("????") },
            { 0xE567, new SpecialCharacter("????") },
            { 0xE568, new SpecialCharacter("????") },
            { 0xE569, new SpecialCharacter("??") },
            { 0xE56A, new SpecialCharacter("????") },
            { 0xE56B, new SpecialCharacter("????") },
            { 0xE56C, new SpecialCharacter("????") },
            { 0xE56D, new SpecialCharacter("????") },
            { 0xE56E, new SpecialCharacter("????") },
            { 0xE56F, new SpecialCharacter("????") },
            { 0xE570, new SpecialCharacter("????") },
            { 0xE571, new SpecialCharacter("??") },
            { 0xE572, new SpecialCharacter("????") },
            { 0xE573, new SpecialCharacter("????") },
            { 0xE574, new SpecialCharacter("????") },
            { 0xE575, new SpecialCharacter("??") },
            { 0xE576, new SpecialCharacter("????") },
            { 0xE577, new SpecialCharacter("????") },
            { 0xE578, new SpecialCharacter("????") },
            { 0xE579, new SpecialCharacter("????") },
            { 0xE57B, new SpecialCharacter("??") },
            { 0xE5E269, new SpecialCharacter("??") },
            { 0xE5E275, new SpecialCharacter("??") }
        };

        private string _fileName = string.Empty;

        public int CodePage { get; set; } = -1;

        public override string Extension => ".pac";

        public virtual bool IsFpc { get; set; }

        public const string NameOfFormat = "PAC (Screen Electronics)";

        public override string Name => NameOfFormat;

        private static readonly byte[] MarkerStartOfUnicode = { 0x1f, 0xef, 0xbb, 0xbf };
        private const byte MarkerEndOfUnicode = 0x2e;
        private const byte MarkerReplaceEndOfUnicode = 0xff;

        private bool DoWritePacHeaderOpt => IsFpc; // Unknown paragraph header. Seems optional both for PAC and FPC: inserted here for expected broader FPC compatibility, including prior versions of SubtitleEdit
        private static readonly byte[] PacHeaderOpt = { 0x80, 0x80, 0x80 };

        public bool Save(string fileName, Subtitle subtitle)
        {
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                return Save(fileName, fs, subtitle);
            }
        }

        public bool Save(string fileName, Stream stream, Subtitle subtitle)
        {
            _fileName = fileName;

            // header
            stream.WriteByte(1);
            for (var i = 1; i < 23; i++)
            {
                stream.WriteByte(0);
            }

            stream.WriteByte(0x60);

            // paragraphs
            var number = 1;
            var firstParagraph = subtitle.Paragraphs.FirstOrDefault();
            if (firstParagraph != null &&
                Math.Abs(firstParagraph.StartTime.TotalMilliseconds) < 0.001 &&
                firstParagraph.EndTime.TotalMilliseconds < 350)
            {
                number = 0;
            }

            for (var index = 0; index < subtitle.Paragraphs.Count; index++)
            {
                var p = subtitle.Paragraphs[index];
                WriteParagraph(stream, p, number, index + 1 == subtitle.Paragraphs.Count);
                number++;
            }

            // footer
            stream.WriteByte(0xff);
            for (var i = 0; i < 11; i++)
            {
                stream.WriteByte(0);
            }

            stream.WriteByte(0x11);
            stream.WriteByte(0);
            var footerBuffer = Encoding.ASCII.GetBytes("dummy end of file");
            stream.Write(footerBuffer, 0, footerBuffer.Length);
            return true;
        }

        private void WriteParagraph(Stream fs, Paragraph p, int number, bool isLast)
        {
            WriteTimeCode(fs, p.StartTime);
            WriteTimeCode(fs, p.EndTime);

            if (CodePage == -1 && !IsFpc)
            {
                GetCodePage(null, 0, 0);
            }

            byte alignment = 2; // center
            var verticalAlignment = (byte)Math.Max(0, Configuration.Settings.SubtitleSettings.PacVerticalBottom + 1 - Utilities.GetNumberOfLines(p.Text));

            var text = p.Text;
            if (text.StartsWith("{\\an1}", StringComparison.Ordinal) || text.StartsWith("{\\an4}", StringComparison.Ordinal) || text.StartsWith("{\\an7}", StringComparison.Ordinal))
            {
                alignment = 1; // left
            }
            else if (text.StartsWith("{\\an3}", StringComparison.Ordinal) || text.StartsWith("{\\an6}", StringComparison.Ordinal) || text.StartsWith("{\\an9}", StringComparison.Ordinal))
            {
                alignment = 0; // right
            }

            if (text.StartsWith("{\\an7}", StringComparison.Ordinal) || text.StartsWith("{\\an8}", StringComparison.Ordinal) || text.StartsWith("{\\an9}", StringComparison.Ordinal))
            {
                verticalAlignment = (byte)Configuration.Settings.SubtitleSettings.PacVerticalTop; // top
            }
            else if (text.StartsWith("{\\an4}", StringComparison.Ordinal) || text.StartsWith("{\\an5}", StringComparison.Ordinal) || text.StartsWith("{\\an6}", StringComparison.Ordinal))
            {
                verticalAlignment = (byte)Configuration.Settings.SubtitleSettings.PacVerticalCenter; // center
            }

            if (text.Length >= 6 && text[0] == '{' && text[5] == '}')
            {
                text = text.Remove(0, 6);
            }

            text = MakePacItalicsAndRemoveOtherTags(text);

            var encoding = GetEncoding(CodePage);
            byte[] textBuffer;

            if (IsFpc)
            {
                textBuffer = GetUnicodeBytes(text, alignment);
            }
            else if (CodePage == CodePageArabic)
            {
                textBuffer = GetArabicBytes(Utilities.FixEnglishTextInRightToLeftLanguage(text, "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"), alignment);
            }
            else if (CodePage == CodePageHebrew)
            {
                textBuffer = GetHebrewBytes(Utilities.FixEnglishTextInRightToLeftLanguage(text, "0123456789abcdefghijklmnopqrstuvwxyz"), alignment);
            }
            else if (CodePage == CodePageLatin || CodePage == CodePageLatinCzech)
            {
                textBuffer = GetLatinBytes(encoding, text, alignment, null);
            }
            else if (CodePage == CodePageCyrillic)
            {
                textBuffer = GetCyrillicBytes(text, alignment);
            }
            else if (CodePage == CodePageGreek)
            {
                textBuffer = GetGreekBytes(text, alignment);
            }
            else if (CodePage == CodePageLatinTurkish)
            {
                textBuffer = GetLatinBytes(encoding, text, alignment, LatinTurkishOverrides);
            }
            else if (CodePage == CodePageChineseTraditional)
            {
                textBuffer = GetW16Bytes(text, alignment, EncodingChineseTraditional);
            }
            else if (CodePage == CodePageChineseSimplified)
            {
                textBuffer = GetW16Bytes(text, alignment, EncodingChineseSimplified);
            }
            else if (CodePage == CodePageKorean)
            {
                textBuffer = GetW16Bytes(text, alignment, EncodingKorean);
            }
            else if (CodePage == CodePageJapanese)
            {
                textBuffer = GetW16Bytes(text, alignment, EncodingJapanese);
            }
            else if (CodePage == CodePageThai)
            {
                textBuffer = encoding.GetBytes(text.Replace('???', '???'));
            }
            else
            {
                textBuffer = encoding.GetBytes(text);
            }

            // write text length
            var length = (ushort)(textBuffer.Length + 4 + (DoWritePacHeaderOpt ? PacHeaderOpt.Length : 0));
            fs.Write(BitConverter.GetBytes(length), 0, 2);

            fs.WriteByte(verticalAlignment); // fs.WriteByte(0x0a); // sometimes 0x0b? - this seems to be vertical alignment - 0 to 11
            if (DoWritePacHeaderOpt)
            {
                fs.Write(PacHeaderOpt, 0, PacHeaderOpt.Length);
            }

            fs.WriteByte(0xfe);
            fs.WriteByte(alignment); //2=centered, 1=left aligned, 0=right aligned, 09=Fount2 (large font),
            //55=safe area override (too long line), 0A=Fount2 + centered, 06=centered + safe area override
            fs.WriteByte(0x03);

            fs.Write(textBuffer, 0, textBuffer.Length);

            if (!isLast)
            {
                fs.WriteByte(0);
                fs.WriteByte((byte)(number % 256));
                fs.WriteByte((byte)(number / 256));
                fs.WriteByte(0x60);
            }
        }

        internal static string MakePacItalicsAndRemoveOtherTags(string text)
        {
            text = HtmlUtil.RemoveOpenCloseTags(text, HtmlUtil.TagFont, HtmlUtil.TagUnderline).Trim();
            if (!text.Contains("<i>", StringComparison.OrdinalIgnoreCase))
            {
                return text;
            }

            text = text.Replace("<I>", "<i>");
            text = text.Replace("</I>", "</i>");
            var sb = new StringBuilder();
            var parts = text.SplitToLines();
            var nextPre = string.Empty;
            foreach (string line in parts)
            {
                string s = nextPre + line.Trim();
                nextPre = string.Empty;
                var noOfStartTags = Utilities.CountTagInText(s, "<i>");
                var noOfEndTags = Utilities.CountTagInText(s, "</i>");
                if (noOfStartTags == 1 && noOfEndTags == 1 && s.StartsWith("<i>", StringComparison.Ordinal) && s.EndsWith("</i>", StringComparison.Ordinal))
                {
                    sb.AppendLine("<" + HtmlUtil.RemoveHtmlTags(s));
                }
                else
                {
                    s = s.Replace("</i>", "___@___");
                    s = s.Replace("<i>", "<");
                    s = s.Replace("___@___", ">");
                    s = s.Replace(" <", "<");
                    s = s.Replace("> ", ">");
                    if (noOfStartTags > noOfEndTags)
                    {
                        s = s.TrimEnd() + ">";
                        nextPre = "<i>";
                    }
                    sb.AppendLine(s);
                }
            }
            return sb.ToString().Trim();
        }

        internal static void WriteTimeCode(Stream fs, TimeCode timeCode)
        {
            // write four bytes time code
            var hours = Math.Max(0, timeCode.Hours);
            var minutes = Math.Max(0, timeCode.Minutes);
            var seconds = Math.Max(0, timeCode.Seconds);
            var milliseconds = Math.Max(0, timeCode.Milliseconds);

            string highPart = $"{hours:00}{minutes:00}";
            byte frames = (byte)MillisecondsToFramesMaxFrameRate(milliseconds);
            string lowPart = $"{seconds:00}{frames:00}";

            int high = int.Parse(highPart);
            if (high < 256)
            {
                fs.WriteByte((byte)high);
                fs.WriteByte(0);
            }
            else
            {
                fs.WriteByte((byte)(high % 256));
                fs.WriteByte((byte)(high / 256));
            }

            int low = int.Parse(lowPart);
            if (low < 256)
            {
                fs.WriteByte((byte)low);
                fs.WriteByte(0);
            }
            else
            {
                fs.WriteByte((byte)(low % 256));
                fs.WriteByte((byte)(low / 256));
            }
        }

        public override bool IsMine(List<string> lines, string fileName)
        {
            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
            {
                try
                {
                    var fi = new FileInfo(fileName);
                    if (fi.Length > 65 && fi.Length < 1024000) // not too small or too big
                    {
                        byte[] buffer = FileUtil.ReadAllBytesShared(fileName);

                        if (buffer[00] == 1 && // These bytes seems to be PAC files... TODO: Verify!
                            buffer[01] == 0 &&
                            buffer[02] == 0 &&
                            buffer[03] == 0 &&
                            buffer[04] == 0 &&
                            buffer[05] == 0 &&
                            buffer[06] == 0 &&
                            buffer[07] == 0 &&
                            buffer[08] == 0 &&
                            buffer[09] == 0 &&
                            buffer[10] == 0 &&
                            buffer[11] == 0 &&
                            buffer[12] == 0 &&
                            buffer[13] == 0 &&
                            buffer[14] == 0 &&
                            buffer[15] == 0 &&
                            buffer[16] == 0 &&
                            buffer[17] == 0 &&
                            buffer[18] == 0 &&
                            buffer[19] == 0 &&
                            buffer[20] == 0 &&
                            //buffer[21] < 10 && // start from number
                            //buffer[22] == 0 &&
                            //(buffer[23] >= 0x60 && buffer[23] <= 0x70) &&
                            fileName.EndsWith(Extension, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        public override string ToText(Subtitle subtitle, string title)
        {
            return "Not supported!";
        }

        public override void LoadSubtitle(Subtitle subtitle, List<string> lines, string fileName)
        {
            _fileName = fileName;
            LoadSubtitle(subtitle, FileUtil.ReadAllBytesShared(fileName));
        }

        public void LoadSubtitle(Subtitle subtitle, byte[] buffer)
        {
            subtitle.Paragraphs.Clear();
            subtitle.Header = null;
            bool usesSecondaryCodePage = UsesSecondaryCodePage(buffer);
            int index = 0;
            while (index < buffer.Length)
            {
                var p = GetPacParagraph(ref index, buffer, usesSecondaryCodePage, subtitle.Paragraphs.Count);
                if (p != null)
                {
                    subtitle.Paragraphs.Add(p);
                }
            }

            subtitle.Renumber();
        }

        private bool UsesSecondaryCodePage(byte[] buffer)
        {
            bool? firstIsSecondary = null;
            int secondaryUse = 0;
            for (int i = 15; i < buffer.Length - 1; i++)
            {
                if (buffer[i] == 0xFE && (buffer[i - 15] == 0x60 || buffer[i - 15] == 0x61 || buffer[i - 12] == 0x60 || buffer[i - 12] == 0x61))
                {
                    bool secondary = (buffer[i + 1] & 0x08) != 0;
                    firstIsSecondary = firstIsSecondary ?? secondary;
                    if (secondary)
                    {
                        secondaryUse++;
                    }
                }
            }

            return secondaryUse > (firstIsSecondary.GetValueOrDefault() ? 1 : 0);
        }

        private double _lastStartTotalSeconds;
        private double _lastEndTotalSeconds;

        private static bool CompareBytes(byte[] buff, int pos, byte[] seq)
        {
            if (buff.Length < pos + seq.Length)
            {
                return false;
            }

            for (int i = 0; i < seq.Length; i++)
            {
                if (buff[pos + i] != seq[i])
                {
                    return false;
                }
            }

            return true;
        }

        private Paragraph GetPacParagraph(ref int index, byte[] buffer, bool usesSecondaryCodePage, int paragraphIndex)
        {
            var isStory = index < 15;
            if (isStory)
            {
                index = 15;
            }

            while (true)
            {
                index++;
                if (index + 20 >= buffer.Length)
                {
                    return null;
                }

                if (buffer[index] == 0xFE)
                {
                    var minus15 = buffer[index - 15];
                    if (minus15 == 0x60 || minus15 == 0x61 || minus15 == 0x62)
                    {
                        break;
                    }

                    var minus12 = buffer[index - 12];
                    if (minus12 == 0x60 || minus12 == 0x61 || minus12 == 0x62)
                    {
                        break;
                    }
                }
            }

            int feIndex = index;
            const int endDelimiter = 0x00;
            byte alignment = buffer[feIndex + 1];
            bool isSecondaryCodePage = (alignment & 0x08) != 0;
            alignment &= 0x07;
            var p = new Paragraph();
            int timeStartIndex = feIndex - 15;
            if (buffer[timeStartIndex] == 0x60)
            {
                p.StartTime = GetTimeCode(timeStartIndex + 1, buffer);
                p.EndTime = GetTimeCode(timeStartIndex + 5, buffer);
            }
            else if (buffer[timeStartIndex + 3] == 0x60)
            {
                timeStartIndex += 3;
                p.StartTime = GetTimeCode(timeStartIndex + 1, buffer);
                p.EndTime = GetTimeCode(timeStartIndex + 5, buffer);
            }
            else if (buffer[timeStartIndex] == 0x61 || buffer[timeStartIndex] == 0x62)
            {
                p.StartTime = GetTimeCode(timeStartIndex + 1, buffer);
                p.EndTime = GetTimeCode(timeStartIndex + 5, buffer);
                int length = buffer[timeStartIndex + 9] + buffer[timeStartIndex + 10] * 256;
                if (length < 1 || length > 200 || (paragraphIndex != 1 &&
                    (p.StartTime.TotalSeconds - _lastStartTotalSeconds < 1 || p.StartTime.TotalSeconds - _lastStartTotalSeconds > 1500 ||
                    p.EndTime.TotalSeconds - _lastEndTotalSeconds < 1 || p.EndTime.TotalSeconds - _lastEndTotalSeconds > 1500)))
                {
                    return null;
                }
            }
            else if (buffer[timeStartIndex + 3] == 0x61 || buffer[timeStartIndex + 3] == 0x62)
            {
                timeStartIndex += 3;
                p.StartTime = GetTimeCode(timeStartIndex + 1, buffer);
                p.EndTime = GetTimeCode(timeStartIndex + 5, buffer);
                int length = buffer[timeStartIndex + 9] + buffer[timeStartIndex + 10] * 256;
                if (length < 1 || length > 200 || (paragraphIndex != 1 &&
                   (p.StartTime.TotalSeconds - _lastStartTotalSeconds < 1 || p.StartTime.TotalSeconds - _lastStartTotalSeconds > 1500 ||
                   p.EndTime.TotalSeconds - _lastEndTotalSeconds < 1 || p.EndTime.TotalSeconds - _lastEndTotalSeconds > 1500)))
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

            int textLength = buffer[timeStartIndex + 9] + buffer[timeStartIndex + 10] * 256;
            if (textLength > 500)
            {
                return null; // probably not correct index
            }

            _lastStartTotalSeconds = p.StartTime.TotalSeconds;
            _lastEndTotalSeconds = p.EndTime.TotalSeconds;

            var maxIndex = timeStartIndex + 10 + textLength;

            var verticalAlignment = buffer[timeStartIndex + 11];

            if (CodePage == -1 && !IsFpc)
            {
                GetCodePage(buffer, index, endDelimiter);
            }

            var overrides = (CodePage == CodePageLatinTurkish) ? LatinTurkishOverrides : null;
            var sb = new StringBuilder();
            index = feIndex + 3;
            var w16 = buffer[index] == 0x1f && Encoding.ASCII.GetString(buffer, index + 1, 3) == "W16";
            if (w16)
            {
                index += 5;
            }

            var isUnicode = false;
            while (index < buffer.Length && index <= maxIndex) // buffer[index] != endDelimiter)
            {
                if (buffer.Length > index + 3 && buffer[index] == 0x1f && Encoding.ASCII.GetString(buffer, index + 1, 3) == "W16")
                {
                    w16 = true;
                    index += 5;
                }
                else if (CompareBytes(buffer, index, MarkerStartOfUnicode))
                {
                    isUnicode = true; IsFpc = true;
                    index += MarkerStartOfUnicode.Length;
                }
                else if (buffer.Length > index + 2 && buffer[index] == 0x1f && buffer[index + 1] == 'C' && char.IsDigit((char)buffer[index + 2]))
                {
                    index += 3;
                    continue;
                }

                if (w16)
                {
                    if (buffer[index] == 0xFE)
                    {
                        alignment = buffer[index + 1];
                        isSecondaryCodePage = (alignment & 0x08) != 0;
                        alignment &= 0x07;
                        sb.AppendLine();
                        w16 = buffer[index + 3] == 0x1f && Encoding.ASCII.GetString(buffer, index + 4, 3) == "W16";
                        if (w16)
                        {
                            index += 5;
                        }

                        index += 2;
                    }
                    else
                    {
                        if (buffer[index] == 0)
                        {
                            sb.Append(Encoding.ASCII.GetString(buffer, index + 1, 1));
                        }
                        else if (buffer.Length > index + 1)
                        {
                            if (CodePage == CodePageChineseSimplified)
                            {
                                sb.Append(Encoding.GetEncoding(EncodingChineseSimplified).GetString(buffer, index, 2));
                            }
                            else if (CodePage == CodePageKorean)
                            {
                                sb.Append(Encoding.GetEncoding(EncodingKorean).GetString(buffer, index, 2));
                            }
                            else if (CodePage == CodePageJapanese)
                            {
                                sb.Append(Encoding.GetEncoding(EncodingJapanese).GetString(buffer, index, 2));
                            }
                            else
                            {
                                sb.Append(Encoding.GetEncoding(EncodingChineseTraditional).GetString(buffer, index, 2));
                            }
                        }

                        index++;
                    }
                }
                else if (buffer[index] == 0xFE)
                {
                    alignment = buffer[index + 1];
                    isSecondaryCodePage = (alignment & 0x08) != 0;
                    alignment &= 0x07;
                    sb.AppendLine();
                    index += 2;
                }
                else if (isUnicode)
                {
                    if (buffer[index] == MarkerEndOfUnicode)
                    {
                        isUnicode = false;
                    }
                    else if (buffer[index] == MarkerReplaceEndOfUnicode)
                    {
                        sb.Append((char)MarkerEndOfUnicode);
                    }
                    else
                    {
                        int len = 1;
                        byte b = buffer[index];
                        if (b >= 0xE0)
                        {
                            len = 3;
                        }
                        else if (b >= 0xC0)
                        {
                            len = 2;
                        }

                        if (buffer.Length > index + len - 1)
                        {
                            sb.Append(Encoding.UTF8.GetString(buffer, index, len));
                        }

                        index += len - 1;
                    }
                }
                else if (buffer[index] == 0xFF)
                {
                    sb.Append(' ');
                }
                else if (CodePage == CodePageLatin || CodePage == CodePageLatinTurkish || CodePage == CodePageLatinCzech
                         || (usesSecondaryCodePage && !isSecondaryCodePage))
                {
                    sb.Append(GetLatinString(GetEncoding(CodePage), buffer, ref index, overrides));
                }
                else if (CodePage == CodePageArabic)
                {
                    sb.Append(GetArabicString(buffer, ref index));
                }
                else if (CodePage == CodePageHebrew)
                {
                    sb.Append(GetHebrewString(buffer, ref index));
                }
                else if (CodePage == CodePageCyrillic)
                {
                    sb.Append(GetCyrillicString(buffer, ref index));
                }
                else if (CodePage == CodePageGreek)
                {
                    sb.Append(GetGreekString(buffer, ref index));
                }
                else if (CodePage == CodePageThai)
                {
                    sb.Append(GetEncoding(CodePage).GetString(buffer, index, 1).Replace("???", "???"));
                }
                else
                {
                    sb.Append(GetEncoding(CodePage).GetString(buffer, index, 1));
                }

                index++;
            }

            if (index + 20 >= buffer.Length)
            {
                return null;
            }

            p.Text = sb.ToString();
            p.Text = p.Text.Replace("\0", string.Empty);
            p.Text = FixItalics(p.Text);
            if (CodePage == CodePageArabic)
            {
                p.Text = Utilities.FixEnglishTextInRightToLeftLanguage(p.Text, "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");
            }
            else if (CodePage == CodePageHebrew)
            {
                p.Text = Utilities.FixEnglishTextInRightToLeftLanguage(p.Text, "0123456789abcdefghijklmnopqrstuvwxyz");
            }

            if (verticalAlignment < 5)
            {
                if (alignment == 1) // left
                {
                    p.Text = "{\\an7}" + p.Text;
                }
                else if (alignment == 0) // right
                {
                    p.Text = "{\\an9}" + p.Text;
                }
                else
                {
                    p.Text = "{\\an8}" + p.Text;
                }
            }
            else if (verticalAlignment < 9)
            {
                if (alignment == 1) // left
                {
                    p.Text = "{\\an4}" + p.Text;
                }
                else if (alignment == 0) // right
                {
                    p.Text = "{\\an6}" + p.Text;
                }
                else
                {
                    p.Text = "{\\an5}" + p.Text;
                }
            }
            else
            {
                if (alignment == 1) // left
                {
                    p.Text = "{\\an1}" + p.Text;
                }
                else if (alignment == 0) // right
                {
                    p.Text = "{\\an3}" + p.Text;
                }
            }

            // Remove position tags
            var indexOfPositioningCodes = p.Text.IndexOf("\u002e\u001f");
            if (indexOfPositioningCodes > 0)
            {
                p.Text = p.Text.Substring(0, indexOfPositioningCodes + 1);
            }

            p.Text = p.Text.RemoveControlCharactersButWhiteSpace().TrimEnd();
            return p;
        }

        /// <summary>
        /// Fix italic tags, lines starting with ">" - whole line is italic, words between &lt;&gt; is italic
        /// </summary>
        private static string FixItalics(string input)
        {
            var pre = string.Empty;
            var text = input;
            if (text.StartsWith("{\\", StringComparison.Ordinal))
            {
                var endIdx = text.IndexOf('}', 2);
                if (endIdx > 2)
                {
                    pre = text.Substring(0, endIdx + 1);
                    text = text.Remove(0, endIdx + 1);
                }
            }
            int index = text.IndexOf('<');
            if (index < 0)
            {
                return input;
            }

            while (index >= 0 && index < text.Length - 1)
            {
                var insertSpace = index >= 1 && text[index - 1] != ' ';

                text = text.Insert(index + 1, "i>");
                if (insertSpace)
                {
                    text = text.Insert(index, " ");
                    index++;
                }

                int indexOfNewLine = text.IndexOf(Environment.NewLine, index + 3, StringComparison.Ordinal);
                int indexOfEnd = text.IndexOf('>', index + 3);
                if (indexOfNewLine < 0 && indexOfEnd < 0)
                {
                    index = -1;
                    text += "</i>";
                }
                else
                {
                    if (indexOfNewLine < 0 || (indexOfEnd > 0 && (indexOfEnd < indexOfNewLine || indexOfEnd == -1)))
                    {
                        insertSpace = indexOfEnd > 1 && text[indexOfEnd - 1] != ' ';
                        text = text.Insert(indexOfEnd, "</i");
                        if (insertSpace)
                        {
                            if (text.Length > indexOfEnd + 4 && ".!?\"".Contains(text[indexOfEnd + 4]))
                            {
                                insertSpace = false;
                            }

                            if (insertSpace)
                            {
                                text = text.Insert(indexOfEnd + 4, " ");
                                indexOfEnd++;
                            }
                        }
                        index = text.IndexOf('<', indexOfEnd + 3);
                    }
                    else
                    {
                        text = text.Insert(indexOfNewLine, "</i>");
                        index = text.IndexOf('<', indexOfNewLine + 4);
                    }
                }
            }

            text = text
                .Replace("  ", " ")
                .Replace("  ", " ")
                .Replace(" " + Environment.NewLine, Environment.NewLine)
                .Replace(" " + Environment.NewLine, Environment.NewLine)
                .Replace(Environment.NewLine + " ", Environment.NewLine)
                .Replace(Environment.NewLine + " ", Environment.NewLine)
                .Replace("</i>" + Environment.NewLine + "<i>", Environment.NewLine)
                .Trim();

            return pre + HtmlUtil.FixInvalidItalicTags(text);
        }

        public static Encoding GetEncoding(int codePage)
        {
            switch (codePage)
            {
                case CodePageLatin:
                    return Encoding.GetEncoding("iso-8859-1");
                case CodePageGreek:
                    return Encoding.GetEncoding("iso-8859-7");
                case CodePageLatinCzech:
                    return Encoding.GetEncoding("iso-8859-2");
                case CodePageArabic:
                    return Encoding.GetEncoding("iso-8859-6");
                case CodePageHebrew:
                    return Encoding.GetEncoding("iso-8859-8");
                case CodePageLatinTurkish:
                    return Encoding.GetEncoding("iso-8859-9");
                case CodePageThai:
                    return Encoding.GetEncoding("windows-874");
                case CodePageCyrillic:
                    return Encoding.GetEncoding("iso-8859-5");
                default:
                    return Encoding.Default;
            }
        }

        public static int AutoDetectEncoding(string fileName)
        {
            var pac = new Pac();
            try
            {
                var dictionary = new Dictionary<int, string>
                {
                    { CodePageLatin, "en-da-no-sv-es-it-fr-pt-de-nl-pl-sq-hr-sr-ro-id" },
                    { CodePageGreek, "el" },
                    { CodePageLatinCzech, "cs-sk" },
                    { CodePageLatinTurkish, "tr" },
                    { CodePageCyrillic, "bg-ru-uk-mk" },
                    { CodePageHebrew, "he" },
                    { CodePageThai, "th" },
                    { CodePageArabic, "ar" },
                    { CodePageKorean, "ko" },
                    { CodePageChineseTraditional, "zh" },
                    { CodePageChineseSimplified, "zh" },
                    { CodePageJapanese, "ja" }
                };
                foreach (var kvp in dictionary)
                {
                    var sub = new Subtitle();
                    pac.CodePage = kvp.Key;
                    pac.LoadSubtitle(sub, null, fileName);
                    var languageCode = LanguageAutoDetect.AutoDetectGoogleLanguageOrNull(sub);
                    if (languageCode != null && kvp.Value.Contains(languageCode))
                    {
                        return kvp.Key;
                    }
                }
            }
            catch
            {
                // ignored
            }

            return CodePageLatin;
        }

        private void GetCodePage(byte[] buffer, int index, int endDelimiter)
        {
            if (BatchMode)
            {
                if (CodePage == -1)
                {
                    CodePage = AutoDetectEncoding(_fileName);
                }

                return;
            }

            byte[] previewBuffer = null;
            if (buffer != null)
            {
                var textSample = new byte[200];
                int textIndex = 0;
                while (index < buffer.Length && buffer[index] != endDelimiter)
                {
                    if (buffer[index] == 0xFF)
                    {
                        if (textIndex < textSample.Length - 1)
                        {
                            textSample[textIndex++] = 32; // ASCII 32 SP (Space)
                        }

                        index++;
                    }
                    else if (buffer[index] == 0xFE)
                    {
                        if (textIndex < textSample.Length - 3)
                        {
                            textSample[textIndex++] = buffer[index];
                            textSample[textIndex++] = buffer[index + 1];
                            textSample[textIndex++] = buffer[index + 2];
                        }
                        index += 3;
                    }
                    else
                    {
                        if (textIndex < textSample.Length - 1)
                        {
                            textSample[textIndex++] = buffer[index];
                        }

                        index++;
                    }
                }

                previewBuffer = new byte[textIndex];
                for (int i = 0; i < textIndex; i++)
                {
                    previewBuffer[i] = textSample[i];
                }
            }

            CodePage = GetPacEncodingImplementation?.GetPacEncoding(previewBuffer, _fileName) ?? 2;
        }

        private static byte[] GetLatinBytes(Encoding encoding, string text, byte alignment, Dictionary<int, SpecialCharacter> extraCharacters)
        {
            int i = 0;
            var buffer = new byte[text.Length * 2];
            int extra = 0;
            while (i < text.Length)
            {
                if (text.Substring(i).StartsWith(Environment.NewLine, StringComparison.Ordinal))
                {
                    buffer[i + extra] = 0xfe;
                    i++;
                    buffer[i + extra] = alignment;
                    extra++;
                    buffer[i + extra] = 3;
                }
                else
                {
                    string letter = text.Substring(i, 1);
                    var code = Find(extraCharacters, letter) ?? Find(LatinCodes, letter);
                    if (code != null)
                    {
                        int byteValue = code.Value.Key;
                        if (byteValue < 256)
                        {
                            buffer[i + extra] = (byte)byteValue;
                        }
                        else
                        {
                            int high = byteValue / 256;
                            int low = byteValue % 256;

                            buffer[i + extra] = (byte)high;
                            extra++;
                            buffer[i + extra] = (byte)low;
                        }
                    }
                    else
                    {
                        var values = encoding.GetBytes(letter);
                        for (int k = 0; k < values.Length; k++)
                        {
                            byte v = values[k];
                            if (k > 0)
                            {
                                extra++;
                            }

                            buffer[i + extra] = v;
                        }
                    }
                }
                i++;
            }

            var result = new byte[i + extra];
            for (int j = 0; j < i + extra; j++)
            {
                result[j] = buffer[j];
            }

            return result;
        }

        private static byte[] GetArabicBytes(string text, byte alignment)
        {
            return GetBytesViaLists(text, ArabicCodes, alignment);
        }

        private static byte[] GetHebrewBytes(string text, byte alignment)
        {
            return GetBytesViaLists(text, HebrewCodes, alignment);
        }

        private static byte[] GetCyrillicBytes(string text, byte alignment)
        {
            return GetBytesViaLists(text, CyrillicCodes, alignment);
        }

        private static byte[] GetGreekBytes(string text, byte alignment)
        {
            return GetBytesViaLists(text, GreekCodes, alignment);
        }

        private static byte[] GetBytesViaLists(string text, Dictionary<int, SpecialCharacter> codes, byte alignment)
        {
            text = text.Replace("???", "'");
            int i = 0;
            var buffer = new byte[text.Length * 2];
            int extra = 0;
            while (i < text.Length)
            {
                if (text.Substring(i).StartsWith(Environment.NewLine, StringComparison.Ordinal))
                {
                    buffer[i + extra] = 0xfe;
                    i++;
                    buffer[i + extra] = alignment;
                    extra++;
                    buffer[i + extra] = 3;
                }
                else
                {
                    bool doubleCharacter = false;
                    string letter = string.Empty;
                    KeyValuePair<int, SpecialCharacter>? character = null;
                    if (i + 1 < text.Length)
                    {
                        letter = text.Substring(i, 2);
                        character = Find(codes, letter);
                        if (character != null)
                        {
                            doubleCharacter = true;
                        }
                    }
                    if (character == null)
                    {
                        letter = text.Substring(i, 1);
                        character = Find(codes, letter);
                    }
                    if (character.HasValue)
                    {
                        int byteValue = character.Value.Key;
                        if (byteValue < 256)
                        {
                            buffer[i + extra] = (byte)byteValue;
                        }
                        else if (byteValue < 65536)
                        {
                            int high = byteValue / 256;
                            int low = byteValue % 256;
                            buffer[i + extra] = (byte)high;
                            if (doubleCharacter)
                            {
                                i++;
                                doubleCharacter = false;
                            }
                            else
                            {
                                extra++;
                            }
                            buffer[i + extra] = (byte)low;
                        }
                        else
                        {
                            int highest = byteValue / 65536;
                            int high = (byteValue / 256) % 256;
                            int low = byteValue % 256;

                            buffer[i + extra] = (byte)highest;
                            extra++;
                            buffer[i + extra] = (byte)high;
                            extra++;
                            buffer[i + extra] = (byte)low;
                        }
                    }
                    else
                    {
                        var values = Encoding.Default.GetBytes(letter);
                        for (int k = 0; k < values.Length; k++)
                        {
                            byte v = values[k];
                            if (k > 0)
                            {
                                extra++;
                            }

                            buffer[i + extra] = v;
                        }
                    }
                    if (doubleCharacter)
                    {
                        i++;
                    }
                }
                i++;
            }

            var result = new byte[i + extra];
            for (int j = 0; j < i + extra; j++)
            {
                result[j] = buffer[j];
            }

            return result;
        }

        private static byte[] GetW16Bytes(string text, byte alignment, int encoding)
        {
            var result = new List<byte>();
            bool firstLine = true;
            foreach (var line in text.SplitToLines())
            {
                if (!firstLine)
                {
                    result.Add(0xfe);
                    result.Add(alignment);
                    result.Add(3);
                }

                if (OnlyAnsi(line))
                {
                    foreach (var b in GetLatinBytes(GetEncoding(CodePageLatin), line, alignment, null))
                    {
                        result.Add(b);
                    }
                }
                else
                {
                    result.Add(0x1f); // ?
                    result.Add(0x57); // W
                    result.Add(0x31); // 1
                    result.Add(0x36); // 6
                    result.Add(0x2e); // ?

                    foreach (var b in Encoding.GetEncoding(encoding).GetBytes(line))
                    {
                        result.Add(b);
                    }
                }
                firstLine = false;
            }
            return result.ToArray();
        }

        private static bool OnlyAnsi(string line)
        {
            string latin = Utilities.AllLettersAndNumbers + " .!?/%:;=()#$'&\"";
            foreach (char ch in line)
            {
                if (!latin.Contains(ch))
                {
                    return false;
                }
            }
            return true;
        }

        private static byte[] GetUnicodeBytes(string text, byte alignment)
        {
            var result = new List<byte>();
            bool firstLine = true;
            foreach (var line in text.SplitToLines())
            {
                if (!firstLine)
                {
                    result.Add(0xfe);
                    result.Add(alignment);
                    result.Add(3);
                }
                result.AddRange(MarkerStartOfUnicode);
                foreach (var b in Encoding.UTF8.GetBytes(line))
                {
                    result.Add(b == MarkerEndOfUnicode ? MarkerReplaceEndOfUnicode : b);
                }
                result.Add(MarkerEndOfUnicode);
                firstLine = false;
            }
            return result.ToArray();
        }

        public static string GetArabicString(byte[] buffer, ref int index)
        {
            var arabicCharacter = GetNextArabicCharacter(buffer, ref index);

            if (arabicCharacter.HasValue && arabicCharacter.Value.SwitchOrder)
            {
                // if we have a special character we must fetch the next one and move it before the current special one
                var tempIndex = index + 1;
                var nextArabicCharacter = GetNextArabicCharacter(buffer, ref tempIndex);
                if (buffer[tempIndex] >= 0x80 && nextArabicCharacter != null)
                {
                    index = tempIndex;
                    var combined = $"{nextArabicCharacter.Value.Character}{arabicCharacter.Value.Character}";
                    return combined;
                }
            }

            if (ThrowOnError && !arabicCharacter.HasValue)
            {
                throw new InvalidOperationException($"Unknown byte ({buffer[index]}) in subtitle file @ binary offset {index}.");
            }

            return arabicCharacter.HasValue
                ? arabicCharacter.Value.Character
                : string.Empty;
        }

        private static SpecialCharacter? GetNextArabicCharacter(byte[] buffer, ref int index)
        {
            if (index >= buffer.Length)
            {
                return null;
            }

            byte b = buffer[index];
            SpecialCharacter? arabicCharacter = null;
            if (ArabicCodes.ContainsKey(b))
            {
                arabicCharacter = ArabicCodes[b];
            }

            if (arabicCharacter != null && buffer.Length > index + 1)
            {
                var code = b * 256 + buffer[index + 1];
                if (ArabicCodes.ContainsKey(code))
                {
                    index++;
                    arabicCharacter = ArabicCodes[code];
                }
            }

            if (arabicCharacter == null && b >= 0x20 && b < 0x70)
            {
                return new SpecialCharacter(Encoding.ASCII.GetString(buffer, index, 1));
            }

            return arabicCharacter;
        }

        public static string GetHebrewString(byte[] buffer, ref int index)
        {
            byte b = buffer[index];
            if (b >= 0x20 && b < 0x70 && b != 44)
            {
                return Encoding.ASCII.GetString(buffer, index, 1);
            }

            if (HebrewCodes.ContainsKey(b))
            {
                return HebrewCodes[b].Character;
            }

            if (ThrowOnError)
            {
                throw new InvalidOperationException($"Unknown byte ({b}) in subtitle file @ binary offset {index}.");
            }

            return string.Empty;
        }

        private static bool TryGetMappedCharacter(Dictionary<int, SpecialCharacter> map, byte[] buffer, ref int index, out string result)
        {
            byte b = buffer[index];

            result = string.Empty;
            if (map.ContainsKey(b))
            {
                result = map[b].Character;
            }

            if (buffer.Length > index + 2)
            {
                var code = b * 256 + buffer[index + 1];
                var next = buffer[index + 1] * 256 + buffer[index + 2];

                if (map.ContainsKey(code) && (!map.ContainsKey(next) || map[code].Priority > map[next].Priority))
                {
                    index++;
                    result = map[code].Character;
                }
            }

            return !string.IsNullOrEmpty(result);
        }

        public static string GetLatinString(Encoding encoding, byte[] buffer, ref int index)
        {
            return GetLatinString(encoding, buffer, ref index, null);
        }

        private static string GetLatinString(Encoding encoding, byte[] buffer, ref int index, Dictionary<int, SpecialCharacter> overrides)
        {
            if (overrides != null && TryGetMappedCharacter(overrides, buffer, ref index, out var result))
            {
                return result;
            }

            if (TryGetMappedCharacter(LatinCodes, buffer, ref index, out result))
            {
                return result;
            }

            if (buffer[index] > 13)
            {
                return encoding.GetString(buffer, index, 1);
            }

            if (ThrowOnError)
            {
                throw new InvalidOperationException($"Unknown byte ({buffer[index]}) in subtitle file @ binary offset {index}.");
            }

            return string.Empty;
        }

        public static string GetCyrillicString(byte[] buffer, ref int index)
        {
            byte b = buffer[index];

            if (b >= 0x30 && b <= 0x39) // decimal digits
            {
                return Encoding.ASCII.GetString(buffer, index, 1);
            }

            if (buffer.Length > index + 1)
            {
                var code = b * 256 + buffer[index + 1];
                if (CyrillicCodes.ContainsKey(code))
                {
                    index++;
                    return CyrillicCodes[code].Character;
                }
            }

            if (CyrillicCodes.ContainsKey(b))
            {
                return CyrillicCodes[b].Character;
            }

            if (ThrowOnError)
            {
                throw new InvalidOperationException($"Unknown byte (0x{b:X2}) in subtitle file @ binary offset {index}.");
            }

            return string.Empty;
        }

        public static string GetGreekString(byte[] buffer, ref int index)
        {
            byte b = buffer[index];

            if (b >= 0x30 && b <= 0x39) // decimal digits
            {
                return Encoding.ASCII.GetString(buffer, index, 1);
            }

            if (GreekCodes.ContainsKey(b))
            {
                return GreekCodes[b].Character;
            }

            if (buffer.Length > index + 2)
            {
                int code = b * 65536 + buffer[index + 1] * 256 + buffer[index + 2];
                if (GreekCodes.ContainsKey(code))
                {
                    index += 2;
                    return GreekCodes[code].Character;
                }

                code = b * 256 + buffer[index + 1];
                if (GreekCodes.ContainsKey(code))
                {
                    index++;
                    return GreekCodes[code].Character;
                }
            }

            if (ThrowOnError)
            {
                throw new InvalidOperationException($"Unknown byte ({b}) in subtitle file @ binary offset {index}.");
            }

            return string.Empty;
        }

        public static string GetKoreanString(byte[] buffer, ref int index)
        {
            byte b = buffer[index];

            if (b >= 0x30 && b <= 0x39) // decimal digits
            {
                return Encoding.ASCII.GetString(buffer, index, 1);
            }

            if (KoreanCodes.ContainsKey(b))
            {
                return KoreanCodes[b].Character;
            }

            if (buffer.Length > index + 1)
            {
                var code = b * 256 + buffer[index + 1];
                if (KoreanCodes.ContainsKey(code))
                {
                    index++;
                    return KoreanCodes[code].Character;
                }
            }

            if (ThrowOnError)
            {
                throw new InvalidOperationException($"Unknown byte ({b}) in subtitle file @ binary offset {index}.");
            }

            return string.Empty;
        }

        internal static TimeCode GetTimeCode(int timeCodeIndex, byte[] buffer)
        {
            if (timeCodeIndex > 0)
            {
                string highPart = $"{buffer[timeCodeIndex] + buffer[timeCodeIndex + 1] * 256:000000}";
                string lowPart = $"{buffer[timeCodeIndex + 2] + buffer[timeCodeIndex + 3] * 256:000000}";

                int hours = int.Parse(highPart.Substring(0, 4));
                int minutes = int.Parse(highPart.Substring(4, 2));
                int seconds = int.Parse(lowPart.Substring(2, 2));
                int frames = int.Parse(lowPart.Substring(4, 2));

                return new TimeCode(hours, minutes, seconds, FramesToMillisecondsMax999(frames));
            }
            return new TimeCode();
        }

        private static KeyValuePair<int, SpecialCharacter>? Find(Dictionary<int, SpecialCharacter> list, string letter)
        {
            return list?.Where(c => c.Value.Character == letter).Cast<KeyValuePair<int, SpecialCharacter>?>().FirstOrDefault();
        }

        internal struct SpecialCharacter
        {
            internal SpecialCharacter(string character, bool switchOrder = false, int priority = 2)
            {
                Character = character;
                SwitchOrder = switchOrder;
                Priority = priority;
            }

            internal string Character { get; set; }
            internal bool SwitchOrder { get; set; }
            internal int Priority { get; set; }
        }

        public bool Save(string fileName, Stream stream, Subtitle subtitle, bool batchMode)
        {
            return Save(fileName, stream, subtitle);
        }
    }
}
