namespace lib
{
    public sealed class Voice
    {
        private readonly string v;
        private Voice(string v) { this.v = v; }
        public override string ToString() => v;
        public static Voice From(string s) => new Voice(s);

        // https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/language-support
        public static Voice de_DE_KatjaNeural { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (de-DE, KatjaNeural)");
        public static Voice en_US_GuyNeural { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (en-US, GuyNeural)");
        public static Voice en_US_JessaNeural { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (en-US, JessaNeural)");
        public static Voice it_IT_ElsaNeural { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (it-IT, ElsaNeural)");
        public static Voice zh_CN_XiaoxiaoNeural { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (zh-CN, XiaoxiaoNeural)");

        public static Voice ar_EG_Hoda { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (ar-EG, Hoda)");
        public static Voice ar_SA_Naayf { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (ar-SA, Naayf)");
        public static Voice bg_BG_Ivan { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (bg-BG, Ivan)");
        public static Voice ca_ES_HerenaRUS { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (ca-ES, HerenaRUS)");
        public static Voice cs_CZ_Jakub { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (cs-CZ, Jakub)");
        public static Voice da_DK_HelleRUS { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (da-DK, HelleRUS)");
        public static Voice de_AT_Michael { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (de-AT, Michael)");
        public static Voice de_CH_Karsten { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (de-CH, Karsten)");
        public static Voice de_DE_Hedda { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (de-DE, Hedda)");
        public static Voice de_DE_HeddaRUS { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (de-DE, HeddaRUS)");
        public static Voice de_DE_Stefan_Apollo { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (de-DE, Stefan, Apollo)");
        public static Voice el_GR_Stefanos { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (el-GR, Stefanos)");
        public static Voice en_AU_Catherine { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (en-AU, Catherine)");
        public static Voice en_AU_HayleyRUS { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (en-AU, HayleyRUS)");
        public static Voice en_CA_Linda { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (en-CA, Linda)");
        public static Voice en_CA_HeatherRUS { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (en-CA, HeatherRUS)");
        public static Voice en_GB_Susan_Apollo { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (en-GB, Susan, Apollo)");
        public static Voice en_GB_HazelRUS { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (en-GB, HazelRUS)");
        public static Voice en_GB_George_Apollo { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (en-GB, George, Apollo)");
        public static Voice en_IE_Sean { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (en-IE, Sean)");
        public static Voice en_IN_Heera_Apollo { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (en-IN, Heera, Apollo)");
        public static Voice en_IN_PriyaRUS { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (en-IN, PriyaRUS)");
        public static Voice en_IN_Ravi_Apollo { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (en-IN, Ravi, Apollo)");
        public static Voice en_US_ZiraRUS { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (en-US, ZiraRUS)");
        public static Voice en_US_JessaRUS { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (en-US, JessaRUS)");
        public static Voice en_US_BenjaminRUS { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (en-US, BenjaminRUS)");
        public static Voice en_US_Jessa24kRUS { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (en-US, Jessa24kRUS)");
        public static Voice en_US_Guy24kRUS { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (en-US, Guy24kRUS)");
        public static Voice es_ES_Laura_Apollo { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (es-ES, Laura, Apollo)");
        public static Voice es_ES_HelenaRUS { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (es-ES, HelenaRUS)");
        public static Voice es_ES_Pablo_Apollo { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (es-ES, Pablo, Apollo)");
        public static Voice es_MX_HildaRUS { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (es-MX, HildaRUS)");
        public static Voice es_MX_Raul_Apollo { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (es-MX, Raul, Apollo)");
        public static Voice fi_FI_HeidiRUS { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (fi-FI, HeidiRUS)");
        public static Voice fr_CA_Caroline { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (fr-CA, Caroline)");
        public static Voice fr_CA_HarmonieRUS { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (fr-CA, HarmonieRUS)");
        public static Voice fr_CH_Guillaume { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (fr-CH, Guillaume)");
        public static Voice fr_FR_Julie_Apollo { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (fr-FR, Julie, Apollo)");
        public static Voice fr_FR_HortenseRUS { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (fr-FR, HortenseRUS)");
        public static Voice fr_FR_Paul_Apollo { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (fr-FR, Paul, Apollo)");
        public static Voice he_IL_Asaf { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (he-IL, Asaf)");
        public static Voice hi_IN_Kalpana_Apollo { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (hi-IN, Kalpana, Apollo)");
        public static Voice hi_IN_Kalpana { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (hi-IN, Kalpana)");
        public static Voice hi_IN_Hemant { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (hi-IN, Hemant)");
        public static Voice hr_HR_Matej { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (hr-HR, Matej)");
        public static Voice hu_HU_Szabolcs { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (hu-HU, Szabolcs)");
        public static Voice id_ID_Andika { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (id-ID, Andika)");
        public static Voice it_IT_Cosimo_Apollo { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (it-IT, Cosimo, Apollo)");
        public static Voice it_IT_LuciaRUS { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (it-IT, LuciaRUS)");
        public static Voice ja_JP_Ayumi_Apollo { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (ja-JP, Ayumi, Apollo)");
        public static Voice ja_JP_Ichiro_Apollo { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (ja-JP, Ichiro, Apollo)");
        public static Voice ja_JP_HarukaRUS { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (ja-JP, HarukaRUS)");
        public static Voice ko_KR_HeamiRUS { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (ko-KR, HeamiRUS)");
        public static Voice ms_MY_Rizwan { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (ms-MY, Rizwan)");
        public static Voice nb_NO_HuldaRUS { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (nb-NO, HuldaRUS)");
        public static Voice nl_NL_HannaRUS { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (nl-NL, HannaRUS)");
        public static Voice pl_PL_PaulinaRUS { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (pl-PL, PaulinaRUS)");
        public static Voice pt_BR_HeloisaRUS { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (pt-BR, HeloisaRUS)");
        public static Voice pt_BR_Daniel_Apollo { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (pt-BR, Daniel, Apollo)");
        public static Voice pt_PT_HeliaRUS { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (pt-PT, HeliaRUS)");
        public static Voice ro_RO_Andrei { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (ro-RO, Andrei)");
        public static Voice ru_RU_Irina_Apollo { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (ru-RU, Irina, Apollo)");
        public static Voice ru_RU_Pavel_Apollo { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (ru-RU, Pavel, Apollo)");
        public static Voice ru_RU_EkaterinaRUS { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (ru-RU, EkaterinaRUS)");
        public static Voice sk_SK_Filip { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (sk-SK, Filip)");
        public static Voice sl_SI_Lado { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (sl-SI, Lado)");
        public static Voice sv_SE_HedvigRUS { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (sv-SE, HedvigRUS)");
        public static Voice ta_IN_Valluvar { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (ta-IN, Valluvar)");
        public static Voice te_IN_Chitra { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (te-IN, Chitra)");
        public static Voice th_TH_Pattara { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (th-TH, Pattara)");
        public static Voice tr_TR_SedaRUS { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (tr-TR, SedaRUS)");
        public static Voice vi_VN_An { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (vi-VN, An)");
        public static Voice zh_CN_HuihuiRUS { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (zh-CN, HuihuiRUS)");
        public static Voice zh_CN_Yaoyao_Apollo { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (zh-CN, Yaoyao, Apollo)");
        public static Voice zh_CN_Kangkang_Apollo { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (zh-CN, Kangkang, Apollo)");
        public static Voice zh_HK_Tracy_Apollo { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (zh-HK, Tracy, Apollo)");
        public static Voice zh_HK_TracyRUS { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (zh-HK, TracyRUS)");
        public static Voice zh_HK_Danny_Apollo { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (zh-HK, Danny, Apollo)");
        public static Voice zh_TW_Yating_Apollo { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (zh-TW, Yating, Apollo)");
        public static Voice zh_TW_HanHanRUS { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (zh-TW, HanHanRUS)");
        public static Voice zh_TW_Zhiwei_Apollo { get; } = Voice.From("Microsoft Server Speech Text to Speech Voice (zh-TW, Zhiwei, Apollo)");
    }
}