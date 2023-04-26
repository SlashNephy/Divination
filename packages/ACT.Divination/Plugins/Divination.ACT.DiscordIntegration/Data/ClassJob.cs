using System.Diagnostics.CodeAnalysis;

#nullable enable
namespace Divination.ACT.DiscordIntegration.Data
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal enum ClassJob : byte
    {
        Unknown = 0,
        GLA = 1,
        PGL = 2,
        MRD = 3,
        LNC = 4,
        ARC = 5,
        CNJ = 6,
        THM = 7,
        CRP = 8,
        BSM = 9,
        ARM = 10,
        GSM = 11,
        LTW = 12,
        WVR = 13,
        ALC = 14,
        CUL = 15,
        MIN = 16,
        BTN = 17,
        FSH = 18,
        PLD = 19,
        MNK = 20,
        WAR = 21,
        DRG = 22,
        BRD = 23,
        WHM = 24,
        BLM = 25,
        ACN = 26,
        SMN = 27,
        SCH = 28,
        ROG = 29,
        NIN = 30,
        MCH = 31,
        DRK = 32,
        AST = 33,
        SAM = 34,
        RDM = 35,
        BLU = 36,
        GNB = 37,
        DNC = 38
    }

    internal static class ClassJobEx
    {
        public static Emoji GetEmoji(this ClassJob job)
        {
            switch (job)
            {
                // Class: TANK
                case ClassJob.GLA:
                    return new Emoji("gla", "728869145042616352");
                case ClassJob.MRD:
                    return new Emoji("mrd", "728869145596133436");

                // Class: HEALER
                case ClassJob.CNJ:
                    return new Emoji("cnj", "728869231084699710");

                // Class: DPS
                case ClassJob.PGL:
                    return new Emoji("pgl", "728869433824641046");
                case ClassJob.LNC:
                    return new Emoji("lnc", "728869434181287976");
                case ClassJob.ROG:
                    return new Emoji("rog", "728870622247256125");
                case ClassJob.ARC:
                    return new Emoji("arc", "728869434109853768");
                case ClassJob.THM:
                    return new Emoji("thm", "728869433816121385");
                case ClassJob.ACN:
                    return new Emoji("acn", "728869434336215060");

                // Job: TANK
                case ClassJob.PLD:
                    return new Emoji("pld", "728863143174078535");
                case ClassJob.WAR:
                    return new Emoji("war", "728863142708379680");
                case ClassJob.DRK:
                    return new Emoji("drk", "728863143371210767");
                case ClassJob.GNB:
                    return new Emoji("gnb", "728863143077347378");

                // Job: HEALER
                case ClassJob.WHM:
                    return new Emoji("whm", "728863268151754782");
                case ClassJob.SCH:
                    return new Emoji("sch", "728863268000628816");
                case ClassJob.AST:
                    return new Emoji("ast", "728863268051091528");

                // Job: Melee DPS
                case ClassJob.MNK:
                    return new Emoji("mnk", "728863694947090582");
                case ClassJob.DRG:
                    return new Emoji("drg", "728863694754152460");
                case ClassJob.NIN:
                    return new Emoji("nin", "728863695010267227");
                case ClassJob.SAM:
                    return new Emoji("sam", "728863694771191880");

                // Job: Physical Ranged DPS
                case ClassJob.BRD:
                    return new Emoji("brd", "728863694859141160");
                case ClassJob.MCH:
                    return new Emoji("mch", "728863694754283523");
                case ClassJob.DNC:
                    return new Emoji("dnc", "728863694871592960");

                // Job: Magical Ranged DPS
                case ClassJob.BLM:
                    return new Emoji("blm", "728863694657683498");
                case ClassJob.SMN:
                    return new Emoji("smn", "728863695068856320");
                case ClassJob.RDM:
                    return new Emoji("rdm", "728863694699888731");
                case ClassJob.BLU:
                    return new Emoji("blu", "728863772411953212");

                // Class: CRAFTER
                case ClassJob.CRP:
                    return new Emoji("crp", "728861622377906261");
                case ClassJob.BSM:
                    return new Emoji("bsm", "728861622575300628");
                case ClassJob.ARM:
                    return new Emoji("arm", "728861622566780948");
                case ClassJob.GSM:
                    return new Emoji("gsm", "728861622944399400");
                case ClassJob.LTW:
                    return new Emoji("ltw", "728861622579494962");
                case ClassJob.WVR:
                    return new Emoji("wvr", "728861622445277216");
                case ClassJob.ALC:
                    return new Emoji("alc", "728861622365585470");
                case ClassJob.CUL:
                    return new Emoji("cul", "728861622214459524");

                // Class: GATHERER
                case ClassJob.MIN:
                    return new Emoji("min", "728861797653676092");
                case ClassJob.BTN:
                    return new Emoji("btn", "728861797196759052");
                case ClassJob.FSH:
                    return new Emoji("fsh", "728861797519589506");
                default:
                    return new Emoji("unknown", "728859087286304789");
            }
        }

        public static string? GetImageKey(this ClassJob job)
        {
            return job == ClassJob.Unknown ? null : $"job_{(byte) job}";
        }
    }
}
