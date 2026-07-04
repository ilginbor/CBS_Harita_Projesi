USE [veri_tabani]
GO
/****** Object:  Table [dbo].[scf]    Script Date: 9.01.2025 12:17:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[scf](
	[id] [int] NOT NULL,
	[domain] [nvarchar](255) NOT NULL,
	[control] [nvarchar](255) NOT NULL,
	[number] [nvarchar](255) NOT NULL,
	[description] [nvarchar](max) NOT NULL,
	[methods_to_comply] [nvarchar](max) NULL,
	[question] [nvarchar](max) NOT NULL,
	[weight] [nvarchar](255) NOT NULL,
	[function_grouping] [nvarchar](255) NOT NULL,
	[scrm_tier1_strategic] [nvarchar](255) NULL,
	[scrm_tier2_operational] [nvarchar](255) NULL,
	[scrm_tier3_tactical] [nvarchar](255) NULL,
	[spcmm0_not_performed] [nvarchar](max) NULL,
	[spcmm1_performed_informally] [nvarchar](max) NULL,
	[spcmm2_planned_tracked] [nvarchar](max) NULL,
	[spcmm3_well_defined] [nvarchar](max) NULL,
	[spcmm4_quantitatively_controlled] [nvarchar](max) NULL,
	[spcmm5_continuously_improving] [nvarchar](max) NULL,
	[ISO_27001_v2013] [nvarchar](255) NULL,
	[ISO_27001_v2022] [nvarchar](255) NULL,
	[ISO_27002_v2013] [nvarchar](255) NULL,
	[ISO_27002_v2022] [nvarchar](255) NULL,
 CONSTRAINT [PK_scf] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[scf_assesment_objectives]    Script Date: 9.01.2025 12:17:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[scf_assesment_objectives](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[scf_id] [int] NOT NULL,
	[scf_number] [nvarchar](255) NOT NULL,
	[number] [nvarchar](255) NOT NULL,
	[description] [nvarchar](255) NOT NULL,
	[origin] [nvarchar](255) NOT NULL,
	[Asset Type_examine/interview/test] [nvarchar](255) NULL,
	[Assessment Procedure] [nvarchar](255) NULL,
	[Expected Result(s)] [nvarchar](255) NULL,
	[Assessment Status_met / not met / not tested / an] [nvarchar](255) NULL,
	[Inherited_y/n] [nvarchar](255) NULL,
	[Assessment Frequency] [nvarchar](255) NULL,
	[Last Date Assessed] [nvarchar](255) NULL,
	[Assessment Performed By] [nvarchar](255) NULL,
 CONSTRAINT [PK_scf_assesment_objectives] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[scf_assesment_scope]    Script Date: 9.01.2025 12:17:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[scf_assesment_scope](
	[id] [int] NOT NULL,
	[scf_id] [int] NOT NULL,
	[tenant_id] [int] NULL,
	[state] [int] NOT NULL,
	[is_selected] [bit] NOT NULL,
 CONSTRAINT [PK_scf_assesment_scope] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[scf_authoritative_sources]    Script Date: 9.01.2025 12:17:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[scf_authoritative_sources](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[geography] [nvarchar](255) NOT NULL,
	[mapping_column_header] [nvarchar](255) NOT NULL,
	[source] [nvarchar](255) NOT NULL,
	[authoritative_source_statutory_regulatory_contractual_in] [nvarchar](255) NOT NULL,
	[version] [nvarchar](255) NOT NULL,
	[url] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_scf_authoritative_sources] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[scf_domain_scope_relations]    Script Date: 9.01.2025 12:17:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[scf_domain_scope_relations](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[scf_domain_id] [int] NOT NULL,
	[tenant_id] [int] NULL,
 CONSTRAINT [PK_scf_domain_scope_relations] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[scf_domains]    Script Date: 9.01.2025 12:17:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[scf_domains](
	[id] [smallint] NOT NULL,
	[domain] [nvarchar](255) NOT NULL,
	[identifier] [nvarchar](3) NOT NULL,
	[principles] [nvarchar](max) NOT NULL,
	[principle_intent] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Sayfa1$] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[scf_risk_catalogs]    Script Date: 9.01.2025 12:17:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[scf_risk_catalogs](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[grouping] [nvarchar](255) NOT NULL,
	[number] [nvarchar](255) NOT NULL,
	[risk] [nvarchar](255) NOT NULL,
	[description] [nvarchar](255) NOT NULL,
	[nist_csf_function] [nvarchar](255) NOT NULL,
	[threats] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_scf_risk_catalogs] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[scf_scf_source_substance_relations]    Script Date: 9.01.2025 12:17:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[scf_scf_source_substance_relations](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[scf_id] [int] NOT NULL,
	[scf_source_substance_id] [int] NOT NULL,
	[scf_source_id] [int] NOT NULL,
	[scf_domain_id] [int] NOT NULL,
 CONSTRAINT [PK_scf_scf_source_substance_relations] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[scf_scope]    Script Date: 9.01.2025 12:17:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[scf_scope](
	[id] [int] NOT NULL,
	[tenant_id] [int] NULL,
	[maturity_level] [int] NULL,
	[target_maturity_level] [int] NULL,
	[state] [nvarchar](10) NULL,
	[control_weight] [smallint] NULL,
 CONSTRAINT [PK_scf_scope] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[scf_source_scope_relations]    Script Date: 9.01.2025 12:17:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[scf_source_scope_relations](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[scf_source_id] [int] NOT NULL,
	[tenant_id] [int] NULL,
 CONSTRAINT [PK_scf_source_scope_relations] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[scf_source_substance]    Script Date: 9.01.2025 12:17:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[scf_source_substance](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[source_id] [int] NOT NULL,
	[source_substance] [nvarchar](50) NOT NULL,
	[header] [nvarchar](max) NULL,
 CONSTRAINT [PK_scf_source_substance] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[scf_test]    Script Date: 9.01.2025 12:17:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[scf_test](
	[SCF Domain] [nvarchar](255) NULL,
	[SCF Control] [nvarchar](255) NULL,
	[SCF #] [nvarchar](255) NULL,
	[Secure Controls Framework (SCF)_Control Description] [nvarchar](max) NULL,
	[Methods To Comply With SCF Controls] [nvarchar](max) NULL,
	[Evidence Request List (ERL) #] [nvarchar](255) NULL,
	[SCF Control Question] [nvarchar](max) NULL,
	[Relative Control Weighting] [float] NULL,
	[Function Grouping] [nvarchar](255) NULL,
	[SCRM_Tier 1_Strategic] [nvarchar](255) NULL,
	[SCRM_Tier 2_Operational] [nvarchar](255) NULL,
	[SCRM_Tier 3_Tactical] [nvarchar](255) NULL,
	[SP-CMM 0_Not Performed] [nvarchar](max) NULL,
	[SP-CMM 1_Performed Informally] [nvarchar](max) NULL,
	[SP-CMM 2_Planned & Tracked] [nvarchar](max) NULL,
	[SP-CMM 3_Well Defined] [nvarchar](max) NULL,
	[SP-CMM 4_Quantitatively Controlled] [nvarchar](max) NULL,
	[SP-CMM 5_Continuously Improving] [nvarchar](max) NULL,
	[AICPA_TSC 2017_(Controls)] [nvarchar](255) NULL,
	[AICPA_TSC 2017_(Points of Focus)] [nvarchar](255) NULL,
	[BSI _Standard 200-1] [nvarchar](255) NULL,
	[CIS_CSC_v8#0] [nvarchar](255) NULL,
	[CIS_CSC v8#0_IG1] [nvarchar](255) NULL,
	[CIS_CSC v8#0_IG2] [nvarchar](255) NULL,
	[CIS_CSC v8#0_IG3] [nvarchar](255) NULL,
	[COBIT_2019] [nvarchar](255) NULL,
	[COSO_v2017] [nvarchar](255) NULL,
	[CSA_CCM_v4] [nvarchar](255) NULL,
	[CSA_IoT SCF_v2] [nvarchar](255) NULL,
	[ENISA_v2#0] [nvarchar](255) NULL,
	[GAPP] [nvarchar](255) NULL,
	[IEC 62443-4-2] [nvarchar](255) NULL,
	[ISO/SAE_21434_v2021] [nvarchar](255) NULL,
	[ISO_22301_v2019] [nvarchar](255) NULL,
	[ISO_27001_v2013] [nvarchar](255) NULL,
	[ISO_27001_v2022] [nvarchar](255) NULL,
	[ISO_27002_v2013] [nvarchar](255) NULL,
	[ISO_27002_v2022] [nvarchar](255) NULL,
	[ISO _27017_v2015] [nvarchar](255) NULL,
	[ISO _27018_v2014] [nvarchar](255) NULL,
	[ISO_27701 _v2019] [nvarchar](255) NULL,
	[ISO_29100_v2011] [nvarchar](255) NULL,
	[ISO_31000_v2009] [nvarchar](255) NULL,
	[ISO_31010_v2009] [nvarchar](255) NULL,
	[MITRE_ATT&CK_10] [nvarchar](255) NULL,
	[MPA _Content Security Program_v5#1] [nvarchar](255) NULL,
	[NIAC_Insurance Data Security Model Law (MDL-668)] [nvarchar](255) NULL,
	[NIST_AI RMF_AI 100-1_v1#0] [nvarchar](255) NULL,
	[NIST Privacy Framework_v1#0] [nvarchar](255) NULL,
	[NIST_SSDF] [nvarchar](255) NULL,
	[NIST_800-37 _rev 2] [nvarchar](255) NULL,
	[NIST_800-39] [nvarchar](255) NULL,
	[NIST_800-53_rev4] [nvarchar](255) NULL,
	[NIST_800-53 rev4_(low)] [nvarchar](255) NULL,
	[NIST_800-53 rev4_(moderate)] [nvarchar](255) NULL,
	[NIST_800-53 rev4_(high)] [nvarchar](255) NULL,
	[NIST_800-53_rev5] [nvarchar](255) NULL,
	[NIST_800-53B_rev5_(privacy)] [nvarchar](255) NULL,
	[NIST_800-53B_rev5_(low)] [nvarchar](255) NULL,
	[NIST_800-53B_rev5_(moderate)] [nvarchar](255) NULL,
	[NIST_800-53B_rev5_(high)] [nvarchar](255) NULL,
	[NIST_800-53_rev5_(NOC)] [nvarchar](255) NULL,
	[NIST_800-63B_(partial mapping)] [nvarchar](255) NULL,
	[NIST_800-82 rev3_LOW _OT Overlay] [nvarchar](255) NULL,
	[NIST_800-82 rev3_MODERATE_OT Overlay] [nvarchar](255) NULL,
	[NIST_800-82 rev3_HIGH _OT Overlay] [nvarchar](255) NULL,
	[NIST_800-160] [nvarchar](255) NULL,
	[NIST_800-161_rev 1] [nvarchar](255) NULL,
	[NIST_800-161_rev 1_C-SCRM Baseline] [nvarchar](255) NULL,
	[NIST_800-161_rev 1_Flow Down] [nvarchar](255) NULL,
	[NIST_800-161_rev 1_Level 1] [nvarchar](255) NULL,
	[NIST_800-161_rev 1_Level 2] [nvarchar](255) NULL,
	[NIST_800-161_rev 1_Level 3] [nvarchar](255) NULL,
	[NIST _800-171_rev 2] [nvarchar](255) NULL,
	[NIST _800-171_rev 3 FPD] [nvarchar](255) NULL,
	[NIST _800-171A] [nvarchar](255) NULL,
	[NIST _800-171A_rev 3 IPD] [nvarchar](255) NULL,
	[NIST_800-172] [nvarchar](255) NULL,
	[NIST_800-218_v1#1] [nvarchar](255) NULL,
	[NIST_CSF_v1#1] [nvarchar](255) NULL,
	[NIST_CSF_v2#0 IPD] [nvarchar](255) NULL,
	[OWASP_Top 10_v2021] [nvarchar](255) NULL,
	[PCIDSS_v3#2] [nvarchar](255) NULL,
	[PCIDSS_v4#0] [nvarchar](255) NULL,
	[PCIDSS_v4#0_SAQ A] [nvarchar](255) NULL,
	[PCIDSS_v4#0_SAQ A-EP] [nvarchar](255) NULL,
	[PCIDSS_v4#0_SAQ B] [nvarchar](255) NULL,
	[PCIDSS_v4#0_SAQ B-IP] [nvarchar](255) NULL,
	[PCIDSS_v4#0_SAQ C] [nvarchar](255) NULL,
	[PCIDSS_v4#0_SAQ C-VT] [nvarchar](255) NULL,
	[PCIDSS_v4#0_SAQ D Merchant] [nvarchar](255) NULL,
	[PCIDSS_v4#0_SAQ D Service Provider] [nvarchar](255) NULL,
	[PCIDSS_v4#0_SAQ P2PE] [nvarchar](255) NULL,
	[Shared Assessments SIG 2023] [nvarchar](255) NULL,
	[SWIFT_CSF_v2023] [nvarchar](255) NULL,
	[TISAX_ISA v5#1#0] [nvarchar](255) NULL,
	[UL_2900-1] [nvarchar](255) NULL,
	[UN_R155] [nvarchar](255) NULL,
	[UN_ECE WP#29] [nvarchar](255) NULL,
	[US_C2M2_v2#1] [nvarchar](max) NULL,
	[US_CERT RMM_v1#2] [nvarchar](255) NULL,
	[US_CISA_CPG_v2022] [nvarchar](255) NULL,
	[US_CJIS Security Policy 5#9] [nvarchar](255) NULL,
	[US_CMMC 2#0_Level 1] [nvarchar](255) NULL,
	[US_CMMC 2#0_Level 2] [nvarchar](255) NULL,
	[US_CMMC 2#0_Level 3] [nvarchar](255) NULL,
	[US_CMMC 2#1 (draft)_Level 1] [nvarchar](255) NULL,
	[US_CMMC 2#1 (draft)_Level 2] [nvarchar](255) NULL,
	[US_CMMC 2#1 (draft)_Level 3] [nvarchar](255) NULL,
	[US_CMS_MARS-E v2#0] [nvarchar](255) NULL,
	[US_COPPA] [nvarchar](255) NULL,
	[US_DFARS_Cybersecurity_252#204-70xx] [nvarchar](255) NULL,
	[US_FACTA] [nvarchar](255) NULL,
	[US_FAR_52#204-21] [nvarchar](255) NULL,
	[US_FAR_52#204-27] [nvarchar](255) NULL,
	[US_FAR_Section 889] [nvarchar](255) NULL,
	[US_FDA_21 CFR Part 11] [nvarchar](255) NULL,
	[US_FedRAMP_R4] [nvarchar](255) NULL,
	[US_FedRAMP_R4 (low)] [nvarchar](255) NULL,
	[US_FedRAMP_R4 (moderate)] [nvarchar](255) NULL,
	[US_FedRAMP_R4 (high)] [nvarchar](255) NULL,
	[US_FedRAMP_R4 (LI-SaaS)] [nvarchar](255) NULL,
	[US_FedRAMP_R5] [nvarchar](255) NULL,
	[US_FedRAMP_R5_(low)] [nvarchar](255) NULL,
	[US_FedRAMP_R5_(moderate)] [nvarchar](255) NULL,
	[US_FedRAMP_R5_(high)] [nvarchar](255) NULL,
	[US_FedRAMP_R5_(LI-SaaS)] [nvarchar](255) NULL,
	[US_FERPA] [nvarchar](255) NULL,
	[US_FFIEC] [nvarchar](255) NULL,
	[US_FINRA] [nvarchar](255) NULL,
	[US_FTC Act] [nvarchar](255) NULL,
	[US_GLBA_CFR 314] [nvarchar](255) NULL,
	[US_HIPAA] [nvarchar](255) NULL,
	[HIPAA - HICP_Small Practice] [nvarchar](255) NULL,
	[HIPAA - HICP_Medium Practice] [nvarchar](255) NULL,
	[HIPAA - HICP_Large Practice] [nvarchar](255) NULL,
	[US_IRS 1075] [nvarchar](255) NULL,
	[US_ITAR Part 120_(limited)] [nvarchar](255) NULL,
	[US_NERC_CIP] [nvarchar](255) NULL,
	[US_NISPOM] [nvarchar](255) NULL,
	[US_NNPI_(unclass)] [nvarchar](255) NULL,
	[US_NSTC_NSPM-33] [nvarchar](255) NULL,
	[US_Privacy Shield] [nvarchar](255) NULL,
	[US_SEC_Cybersecurity Rule] [nvarchar](255) NULL,
	[US_SOX] [nvarchar](255) NULL,
	[US_SSA _EIESR_v8#0] [nvarchar](255) NULL,
	[US_StateRAMP_Low_Category 1] [nvarchar](255) NULL,
	[US_StateRAMP_Low+_Category 2] [nvarchar](255) NULL,
	[US_StateRAMP_Moderate_Category 3] [nvarchar](255) NULL,
	[US_TSA / DHS_1580/82-2022-01] [nvarchar](255) NULL,
	[US - AK_PIPA] [nvarchar](255) NULL,
	[US - CA_SB327] [nvarchar](255) NULL,
	[US-CA_CPRA_(Nov 2022)] [nvarchar](255) NULL,
	[US - CA_SB1386] [nvarchar](255) NULL,
	[US - CO_Colorado Privacy Act] [nvarchar](255) NULL,
	[US - IL_BIPA] [nvarchar](255) NULL,
	[US - IL_IPA] [nvarchar](255) NULL,
	[US - IL_PIPA] [nvarchar](255) NULL,
	[US-MA_201 CMR 17#00] [nvarchar](255) NULL,
	[US - NV_SB220] [nvarchar](255) NULL,
	[US - NY_DFS_23 NYCRR500] [nvarchar](255) NULL,
	[US - NY_SHIELD Act_S5575B] [nvarchar](max) NULL,
	[US - OR_646A] [nvarchar](255) NULL,
	[US - SC_Insurance Data Security Act] [nvarchar](255) NULL,
	[US - TX_BC521] [nvarchar](255) NULL,
	[US-TX_Cybersecurity Act] [nvarchar](255) NULL,
	[US-TX DIR Control Standards 2#0] [nvarchar](255) NULL,
	[US-TX_TX-RAMP_Level 1] [nvarchar](255) NULL,
	[US-TX_TX-RAMP_Level 2] [nvarchar](255) NULL,
	[US-TX_SB820] [nvarchar](255) NULL,
	[US-VA_CDPA_2023] [nvarchar](255) NULL,
	[US-VT_Act 171 of 2018] [nvarchar](255) NULL,
	[EMEA_EU_EBA_GL/2019/04] [nvarchar](255) NULL,
	[EMEA_EU_DORA] [nvarchar](255) NULL,
	[EMEA_EU ePrivacy_(draft)] [nvarchar](255) NULL,
	[EMEA_EU_GDPR] [nvarchar](255) NULL,
	[EMEA_EU_NIS2] [nvarchar](255) NULL,
	[EMEA_EU_PSD2] [nvarchar](255) NULL,
	[EMEA_EU_EU-US Data Privacy Framework] [nvarchar](255) NULL,
	[EMEA_Austria] [nvarchar](255) NULL,
	[EMEA_Belgium] [nvarchar](255) NULL,
	[EMEA_Czech Republic] [nvarchar](255) NULL,
	[EMEA_Denmark] [nvarchar](255) NULL,
	[EMEA_Finland] [nvarchar](255) NULL,
	[EMEA_France] [nvarchar](255) NULL,
	[EMEA_Germany] [nvarchar](255) NULL,
	[EMEA_Germany_Banking Supervisory Requirements for IT (BAIT)] [nvarchar](255) NULL,
	[EMEA_Germany_C5-2020] [nvarchar](255) NULL,
	[EMEA_Greece] [nvarchar](255) NULL,
	[EMEA_Hungary] [nvarchar](255) NULL,
	[EMEA_Ireland] [nvarchar](255) NULL,
	[EMEA_Israel_CDMO_v1#0] [nvarchar](255) NULL,
	[EMEA_Israel] [nvarchar](255) NULL,
	[EMEA_Italy] [nvarchar](255) NULL,
	[EMEA_Kenya_DPA 2019] [nvarchar](255) NULL,
	[EMEA_Luxembourg] [nvarchar](255) NULL,
	[EMEA_Netherlands] [nvarchar](255) NULL,
	[EMEA_Nigeria_DPR 2019] [nvarchar](255) NULL,
	[EMEA_Norway] [nvarchar](255) NULL,
	[EMEA_Poland] [nvarchar](255) NULL,
	[EMEA_Portugal] [nvarchar](255) NULL,
	[EMEA_Qatar_PDPPL] [nvarchar](255) NULL,
	[EMEA_Russia] [nvarchar](255) NULL,
	[EMEA_Saudi Arabia_Critical Security Controls] [nvarchar](255) NULL,
	[EMEA_Saudi Arabia_SACS-002] [nvarchar](255) NULL,
	[EMEA_Saudi Arabia_SAMA CSFv1#0] [nvarchar](255) NULL,
	[EMEA_Saudi Arabia_ECC-12018] [nvarchar](255) NULL,
	[EMEA_Saudi Arabia_OTCC-1 2022] [nvarchar](255) NULL,
	[EMEA_Serbia_87/2018] [nvarchar](255) NULL,
	[EMEA_Slovak Republic] [nvarchar](255) NULL,
	[EMEA_South Africa] [nvarchar](255) NULL,
	[EMEA_Spain] [nvarchar](255) NULL,
	[EMEA_Spain_CCN-STIC 825] [nvarchar](255) NULL,
	[EMEA_Sweden] [nvarchar](255) NULL,
	[EMEA_Switzerland] [nvarchar](255) NULL,
	[EMEA_Turkey] [nvarchar](255) NULL,
	[EMEA_UAE] [nvarchar](255) NULL,
	[EMEA_UK_CAF v3#1] [nvarchar](255) NULL,
	[EMEA_UK_CAP 1850] [nvarchar](255) NULL,
	[EMEA_UK_Cyber Essentials] [nvarchar](255) NULL,
	[EMEA_UK_DPA] [nvarchar](255) NULL,
	[EMEA_UK_GDPR] [nvarchar](255) NULL,
	[APAC_Australia_Essential 8_ML 1] [nvarchar](255) NULL,
	[APAC_Australia_Essential 8_ML 2] [nvarchar](255) NULL,
	[APAC_Australia_Essential 8_ML 3] [nvarchar](255) NULL,
	[APAC_Australia_Privacy Act] [nvarchar](255) NULL,
	[APAC_Australian Privacy Principles] [nvarchar](255) NULL,
	[APAC_Australia_ISM 2022] [nvarchar](255) NULL,
	[APAC_Australia_IoT Code of Practice] [nvarchar](255) NULL,
	[APAC_Australia_Prudential Standard CPS230] [nvarchar](255) NULL,
	[APAC_Australia_Prudential Standard CPS234] [nvarchar](255) NULL,
	[APAC_China_Data Security Law (DSL)] [nvarchar](255) NULL,
	[APAC_China_DNSIP] [nvarchar](255) NULL,
	[APAC_China_Privacy Law] [nvarchar](255) NULL,
	[APAC_Hong Kong] [nvarchar](255) NULL,
	[APAC_India_ITR] [nvarchar](255) NULL,
	[APAC_Indonesia] [nvarchar](255) NULL,
	[APAC_Japan_APPI] [nvarchar](255) NULL,
	[APAC_Japan_ISMAP] [nvarchar](255) NULL,
	[APAC_Malaysia] [nvarchar](255) NULL,
	[APAC_New Zealand Health ISF] [nvarchar](255) NULL,
	[APAC_New Zealand_NZISM 3#6] [nvarchar](max) NULL,
	[APAC_New Zealand Privacy Act of 2020] [nvarchar](255) NULL,
	[APAC_Philippines] [nvarchar](255) NULL,
	[APAC_Singapore] [nvarchar](255) NULL,
	[APAC_Singapore _Cyber Hygiene Practice] [nvarchar](255) NULL,
	[APAC_Singapore MAS_TRM 2021] [nvarchar](255) NULL,
	[APAC_South Korea] [nvarchar](255) NULL,
	[APAC_Taiwan] [nvarchar](255) NULL,
	[Americas_Argentina] [nvarchar](255) NULL,
	[Americas_Argentina_Reg 132-2018] [nvarchar](255) NULL,
	[Americas_Bahamas] [nvarchar](255) NULL,
	[Americas_Bermuda_BMACCC] [nvarchar](255) NULL,
	[Americas_Brazil_LGPD] [nvarchar](255) NULL,
	[Americas_Canada_CSAG] [nvarchar](255) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[scf_threats]    Script Date: 9.01.2025 12:17:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[scf_threats](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[grouping] [nvarchar](255) NOT NULL,
	[number] [nvarchar](255) NOT NULL,
	[threat] [nvarchar](255) NOT NULL,
	[description] [nvarchar](max) NOT NULL,
	[risks] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_scf_threats] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[scf_assesment_scope] ADD  CONSTRAINT [DF_scf_assesment_scope_state]  DEFAULT ((-1)) FOR [state]
GO
ALTER TABLE [dbo].[scf_assesment_scope] ADD  CONSTRAINT [DF_scf_assesment_scope_is_selected]  DEFAULT ((0)) FOR [is_selected]
GO
ALTER TABLE [dbo].[scf_scope] ADD  CONSTRAINT [DF_scf_scope_state]  DEFAULT (N'Deficient') FOR [state]
GO
ALTER TABLE [dbo].[scf_scf_source_substance_relations]  WITH CHECK ADD  CONSTRAINT [FK_scf_scf_source_substance_relations_scf] FOREIGN KEY([scf_id])
REFERENCES [dbo].[scf] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[scf_scf_source_substance_relations] CHECK CONSTRAINT [FK_scf_scf_source_substance_relations_scf]
GO
ALTER TABLE [dbo].[scf_scf_source_substance_relations]  WITH CHECK ADD  CONSTRAINT [FK_scf_scf_source_substance_relations_scf_authoritative_sources] FOREIGN KEY([scf_source_id])
REFERENCES [dbo].[scf_authoritative_sources] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[scf_scf_source_substance_relations] CHECK CONSTRAINT [FK_scf_scf_source_substance_relations_scf_authoritative_sources]
GO
ALTER TABLE [dbo].[scf_source_substance]  WITH CHECK ADD  CONSTRAINT [FK_scf_source_substance_scf_authoritative_sources] FOREIGN KEY([source_id])
REFERENCES [dbo].[scf_authoritative_sources] ([id])
GO
ALTER TABLE [dbo].[scf_source_substance] CHECK CONSTRAINT [FK_scf_source_substance_scf_authoritative_sources]
GO
