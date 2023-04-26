using System.Diagnostics.CodeAnalysis;

namespace Divination.DiscordIntegration.Data
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal enum ClassJob : uint
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
        public static (string, string) GetEmoji(this ClassJob job)
        {
            switch (job)
            {
                // Class: TANK
                case ClassJob.GLA:
                    return ("gla", "728869145042616352");
                case ClassJob.MRD:
                    return ("mrd", "728869145596133436");

                // Class: HEALER
                case ClassJob.CNJ:
                    return ("cnj", "728869231084699710");

                // Class: DPS
                case ClassJob.PGL:
                    return ("pgl", "728869433824641046");
                case ClassJob.LNC:
                    return ("lnc", "728869434181287976");
                case ClassJob.ROG:
                    return ("rog", "728870622247256125");
                case ClassJob.ARC:
                    return ("arc", "728869434109853768");
                case ClassJob.THM:
                    return ("thm", "728869433816121385");
                case ClassJob.ACN:
                    return ("acn", "728869434336215060");

                // Job: TANK
                case ClassJob.PLD:
                    return ("pld", "728863143174078535");
                case ClassJob.WAR:
                    return ("war", "728863142708379680");
                case ClassJob.DRK:
                    return ("drk", "728863143371210767");
                case ClassJob.GNB:
                    return ("gnb", "728863143077347378");

                // Job: HEALER
                case ClassJob.WHM:
                    return ("whm", "728863268151754782");
                case ClassJob.SCH:
                    return ("sch", "728863268000628816");
                case ClassJob.AST:
                    return ("ast", "728863268051091528");

                // Job: Melee DPS
                case ClassJob.MNK:
                    return ("mnk", "728863694947090582");
                case ClassJob.DRG:
                    return ("drg", "728863694754152460");
                case ClassJob.NIN:
                    return ("nin", "728863695010267227");
                case ClassJob.SAM:
                    return ("sam", "728863694771191880");

                // Job: Physical Ranged DPS
                case ClassJob.BRD:
                    return ("brd", "728863694859141160");
                case ClassJob.MCH:
                    return ("mch", "728863694754283523");
                case ClassJob.DNC:
                    return ("dnc", "728863694871592960");

                // Job: Magical Ranged DPS
                case ClassJob.BLM:
                    return ("blm", "728863694657683498");
                case ClassJob.SMN:
                    return ("smn", "728863695068856320");
                case ClassJob.RDM:
                    return ("rdm", "728863694699888731");
                case ClassJob.BLU:
                    return ("blu", "728863772411953212");

                // Class: CRAFTER
                case ClassJob.CRP:
                    return ("crp", "728861622377906261");
                case ClassJob.BSM:
                    return ("bsm", "728861622575300628");
                case ClassJob.ARM:
                    return ("arm", "728861622566780948");
                case ClassJob.GSM:
                    return ("gsm", "728861622944399400");
                case ClassJob.LTW:
                    return ("ltw", "728861622579494962");
                case ClassJob.WVR:
                    return ("wvr", "728861622445277216");
                case ClassJob.ALC:
                    return ("alc", "728861622365585470");
                case ClassJob.CUL:
                    return ("cul", "728861622214459524");

                // Class: GATHERER
                case ClassJob.MIN:
                    return ("min", "728861797653676092");
                case ClassJob.BTN:
                    return ("btn", "728861797196759052");
                case ClassJob.FSH:
                    return ("fsh", "728861797519589506");
                default:
                    return ("unknown", "728859087286304789");
            }
        }

        public static string? GetImageKey(this ClassJob job)
        {
            return job == ClassJob.Unknown ? null : $"job_{(byte)job}";
        }
    }
}
