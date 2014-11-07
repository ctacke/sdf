using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Media.WaveAudio
{
    public enum FormatTag : short
    {
        UNKNOWN = 0x0000,  /*  Microsoft Corporation  */
        PCM = 0x0001,
        ADPCM = 0x0002,  /*  Microsoft Corporation  */
        IEEE_FLOAT = 0x0003,  /*  Microsoft Corporation  */
        /*  IEEE754: range (+1, -1]  */
        /*  32-bit/64-bit format as defined by */
        /*  MSVC++ float/double type */
        IBM_CVSD = 0x0005,  /*  IBM Corporation  */
        ALAW = 0x0006,  /*  Microsoft Corporation  */
        MULAW = 0x0007,  /*  Microsoft Corporation  */
        WMAVOICE9 = 0x000a,  /*  Microsoft Corporation  */
        OKI_ADPCM = 0x0010,  /*  OKI  */
        DVI_ADPCM = 0x0011,  /*  Intel Corporation  */
        IMA_ADPCM = (DVI_ADPCM), /*  Intel Corporation  */
        MEDIASPACE_ADPCM = 0x0012,  /*  Videologic  */
        SIERRA_ADPCM = 0x0013,  /*  Sierra Semiconductor Corp  */
        G723_ADPCM = 0x0014,  /*  Antex Electronics Corporation  */
        DIGISTD = 0x0015,  /*  DSP Solutions, Inc.  */
        DIGIFIX = 0x0016,  /*  DSP Solutions, Inc.  */
        DIALOGIC_OKI_ADPCM = 0x0017,  /*  Dialogic Corporation  */
        MEDIAVISION_ADPCM = 0x0018,  /*  Media Vision, Inc. */
        YAMAHA_ADPCM = 0x0020,  /*  Yamaha Corporation of America  */
        SONARC = 0x0021,  /*  Speech Compression  */
        DSPGROUP_TRUESPEECH = 0x0022,  /*  DSP Group, Inc  */
        ECHOSC1 = 0x0023,  /*  Echo Speech Corporation  */
        AUDIOFILE_AF36 = 0x0024,  /*    */
        APTX = 0x0025,  /*  Audio Processing Technology  */
        AUDIOFILE_AF10 = 0x0026,  /*    */
        DOLBY_AC2 = 0x0030,  /*  Dolby Laboratories  */
        GSM610 = 0x0031,  /*  Microsoft Corporation  */
        MSNAUDIO = 0x0032,  /*  Microsoft Corporation  */
        ANTEX_ADPCME = 0x0033,  /*  Antex Electronics Corporation  */
        CONTROL_RES_VQLPC = 0x0034,  /*  Control Resources Limited  */
        DIGIREAL = 0x0035,  /*  DSP Solutions, Inc.  */
        DIGIADPCM = 0x0036,  /*  DSP Solutions, Inc.  */

        CONTROL_RES_CR10 = 0x0037,  /*  Control Resources Limited  */
        NMS_VBXADPCM = 0x0038,  /*  Natural MicroSystems  */
        CS_IMAADPCM = 0x0039, /* Crystal Semiconductor IMA ADPCM */
        ECHOSC3 = 0x003A, /* Echo Speech Corporation */
        ROCKWELL_ADPCM = 0x003B,  /* Rockwell International */
        ROCKWELL_DIGITALK = 0x003C,  /* Rockwell International */
        XEBEC = 0x003D,  /* Xebec Multimedia Solutions Limited */
        G721_ADPCM = 0x0040,  /*  Antex Electronics Corporation  */
        G728_CELP = 0x0041,  /*  Antex Electronics Corporation  */
        MPEG = 0x0050,  /*  Microsoft Corporation  */
        MPEGLAYER3 = 0x0055,  /*  ISO/MPEG Layer3 Format Tag */
        CIRRUS = 0x0060,  /*  Cirrus Logic  */
        ESPCM = 0x0061,  /*  ESS Technology  */
        VOXWARE = 0x0062,  /*  Voxware Inc  */
        WAVEFORMAT_CANOPUS_ATRAC = 0x0063,  /*  Canopus, co., Ltd.  */
        G726_ADPCM = 0x0064,  /*  APICOM  */
        G722_ADPCM = 0x0065,  /*  APICOM      */
        DSAT = 0x0066,  /*  Microsoft Corporation  */
        DSAT_DISPLAY = 0x0067,  /*  Microsoft Corporation  */
        SOFTSOUND = 0x0080,  /*  Softsound, Ltd.      */
        RHETOREX_ADPCM = 0x0100,  /*  Rhetorex Inc  */
        MSAUDIO1 = 0x0160,  /*  Microsoft Corporation  */
        WMAUDIO2 = 0x0161,  /*  Microsoft Corporation  */
        WMAUDIO3 = 0x0162,  /*  Microsoft Corporation  */
        WMAUDIO_LOSSLESS = 0x0163,  /*  Microsoft Corporation  */
        CREATIVE_ADPCM = 0x0200,  /*  Creative Labs, Inc  */
        CREATIVE_FASTSPEECH8 = 0x0202,  /*  Creative Labs, Inc  */
        CREATIVE_FASTSPEECH10 = 0x0203,  /*  Creative Labs, Inc  */
        QUARTERDECK = 0x0220, /*  Quarterdeck Corporation  */
        FM_TOWNS_SND = 0x0300,  /*  Fujitsu Corp.  */
        BTV_DIGITAL = 0x0400,  /*  Brooktree Corporation  */
        OLIGSM = 0x1000,  /*  Ing C. Olivetti & C., S.p.A.  */
        OLIADPCM = 0x1001,  /*  Ing C. Olivetti & C., S.p.A.  */
        OLICELP = 0x1002,  /*  Ing C. Olivetti & C., S.p.A.  */
        OLISBC = 0x1003,  /*  Ing C. Olivetti & C., S.p.A.  */
        OLIOPR = 0x1004,  /*  Ing C. Olivetti & C., S.p.A.  */
        LH_CODEC = 0x1100,  /*  Lernout & Hauspie  */
        NORRIS = 0x1400,  /*  Norris Communications, Inc.  */
        MIDI = 0x3000,

    }
}
