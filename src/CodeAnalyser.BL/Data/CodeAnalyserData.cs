using CodeAnalyser.BL.Models;
using System.Diagnostics.CodeAnalysis;

namespace CodeAnalyser.BL.Data;

public static class CodeAnalyserData
{
	private static ApplicationIdentifierDescriptor[] s_applicationIdentifierDescriptors;
	private static CodeParsingTemplateDescriptor[] s_codeParsingTemplateDescriptors;
	private static CodeVerificationTemplateDescriptor[] s_codeVerificationTemplateDescriptors;

	public static IEnumerable<ApplicationIdentifierDescriptor> ApplicationIdentifierDescriptors =>
		s_applicationIdentifierDescriptors;

	public static IEnumerable<CodeParsingTemplateDescriptor> CodeParsingTemplateDescriptors =>
		s_codeParsingTemplateDescriptors;

	public static IEnumerable<CodeVerificationTemplateDescriptor> CodeVerificationTemplateDescriptors =>
		s_codeVerificationTemplateDescriptors;

	static CodeAnalyserData()
	{
		InitializeApplicationIdentifierDescriptors();
		InitializeCodeParsingTemplateDescriptors();
		InitializeCodeVerificationTemplateDescriptors();
	}

	[MemberNotNull(nameof(s_applicationIdentifierDescriptors))]
	private static void InitializeApplicationIdentifierDescriptors()
	{
		s_applicationIdentifierDescriptors =
			[
				new ApplicationIdentifierDescriptor(
					ApplicationIdentifier.AI00,
					"Серийный грузовой контейнерный код",
					"SSCC",
					new ApplicationIdentifierConstraints(18, 18, true, false, false)),

				new ApplicationIdentifierDescriptor(
					ApplicationIdentifier.AI01,
					"Идентификационный номер единицы товара",
					"GTIN",
					new ApplicationIdentifierConstraints(14, 14, true, false, false)),

				new ApplicationIdentifierDescriptor(
					ApplicationIdentifier.AI21,
					"Серийный номер",
					"SERIAL",
					new ApplicationIdentifierConstraints(1, 20, true, true, true)),

				new ApplicationIdentifierDescriptor(
					ApplicationIdentifier.AI3103,
					"Нетто вес",
					"NET WEIGHT (kg)",
					new ApplicationIdentifierConstraints(6, 6, true, false, false)),

				new ApplicationIdentifierDescriptor(
					ApplicationIdentifier.AI3350,
					"Брутто объем",
					"VOLUME (l), log",
					new ApplicationIdentifierConstraints(6, 6, true, false, false)),

				new ApplicationIdentifierDescriptor(
					ApplicationIdentifier.AI3351,
					"Брутто объем",
					"VOLUME (l), log",
					new ApplicationIdentifierConstraints(6, 6, true, false, false)),

				new ApplicationIdentifierDescriptor(
					ApplicationIdentifier.AI3352,
					"Брутто объем",
					"VOLUME (l), log",
					new ApplicationIdentifierConstraints(6, 6, true, false, false)),

				new ApplicationIdentifierDescriptor(
					ApplicationIdentifier.AI3353,
					"Брутто объем",
					"VOLUME (l), log",
					new ApplicationIdentifierConstraints(6, 6, true, false, false)),

				new ApplicationIdentifierDescriptor(
					ApplicationIdentifier.AI3354,
					"Брутто объем",
					"VOLUME (l), log",
					new ApplicationIdentifierConstraints(6, 6, true, false, false)),

				new ApplicationIdentifierDescriptor(
					ApplicationIdentifier.AI3355,
					"Брутто объем",
					"VOLUME (l), log",
					new ApplicationIdentifierConstraints(6, 6, true, false, false)),

				new ApplicationIdentifierDescriptor(
					ApplicationIdentifier.AI8005,
					"Цена единицы измерения товара",
					"PRICE PER UNIT",
					new ApplicationIdentifierConstraints(4, 6, true, false, false)),

				new ApplicationIdentifierDescriptor(
					ApplicationIdentifier.AI91,
					"Внутренняя информация компании",
					"INTERNAL",
					new ApplicationIdentifierConstraints(1, 90, true, true, true)),

				new ApplicationIdentifierDescriptor(
					ApplicationIdentifier.AI92,
					"Внутренняя информация компании",
					"INTERNAL",
					new ApplicationIdentifierConstraints(1, 90, true, true, true)),

				new ApplicationIdentifierDescriptor(
					ApplicationIdentifier.AI93,
					"Внутренняя информация компании",
					"INTERNAL",
					new ApplicationIdentifierConstraints(1, 90, true, true, true))
			];
	}

	[MemberNotNull(nameof(s_codeParsingTemplateDescriptors))]
	private static void InitializeCodeParsingTemplateDescriptors()
	{
		s_codeParsingTemplateDescriptors =
			[
				new CodeParsingTemplateDescriptor(
					CodeParsingTemplate.RuTobaccoProducts,
					"Табачная продукция",
					[
						new CodeParsingTemplateUnit(ApplicationIdentifier.AI01, 14),
						new CodeParsingTemplateUnit(ApplicationIdentifier.AI21, 7),
						new CodeParsingTemplateUnit(ApplicationIdentifier.AI8005, 4),
						new CodeParsingTemplateUnit(ApplicationIdentifier.AI93, 4)
					]),

				new CodeParsingTemplateDescriptor(
					CodeParsingTemplate.RuAlternativeTobaccoProductsNonGS1,
					"Альтернативная табачная продукция (non GS1)",
					[
						new CodeParsingTemplateUnit(ApplicationIdentifier.AI01, 14),
						new CodeParsingTemplateUnit(ApplicationIdentifier.AI21, 7),
						new CodeParsingTemplateUnit(ApplicationIdentifier.AI8005, 4),
						new CodeParsingTemplateUnit(ApplicationIdentifier.AI93, 4)
					]),

				new CodeParsingTemplateDescriptor(
					CodeParsingTemplate.RuNicotineContainingProducts,
					"Никотиносодержащая продукция",
					[
						new CodeParsingTemplateUnit(ApplicationIdentifier.AI01, 14),
						new CodeParsingTemplateUnit(ApplicationIdentifier.AI21, 7),
						new CodeParsingTemplateUnit(ApplicationIdentifier.AI8005, 4),
						new CodeParsingTemplateUnit(ApplicationIdentifier.AI93, 4)
					])
			];
	}

	[MemberNotNull(nameof(s_codeVerificationTemplateDescriptors))]
	private static void InitializeCodeVerificationTemplateDescriptors()
	{
		s_codeVerificationTemplateDescriptors =
			[
				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuTobaccoProducts,
					"Табачная продукция",
					false,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 7),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI8005, 4),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuTobaccoProductsGroupPack,
					"Табачная продукция (групповая упаковка)",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 7),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI8005, 6),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuAlternativeTobaccoProducts,
					"Альтернативная табачная продукция",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 7),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI8005, 6),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuAlternativeTobaccoProductsNonGS1,
					"Альтернативная табачная продукция (non GS1)",
					false,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 7),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI8005, 4),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuAlternativeTobaccoProductsGroupPack,
					"Альтернативная табачная продукция (групповая упаковка)",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 7),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI8005, 6),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuNicotineContainingProducts,
					"Никотиносодержащая продукция",
					false,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 7),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI8005, 4),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuNicotineContainingProductsGroupPack,
					"Никотиносодержащая продукция (групповая упаковка)",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 7),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI8005, 6),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuBeerProducts,
					"Пивная продукция",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 7),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuBeerProductsWithVolume,
					"Пивная продукция + объем",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 7),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI3350, default, default,
							[
								new CodeVerificationTemplateUnit(ApplicationIdentifier.AI3351),
								new CodeVerificationTemplateUnit(ApplicationIdentifier.AI3352),
								new CodeVerificationTemplateUnit(ApplicationIdentifier.AI3353),
								new CodeVerificationTemplateUnit(ApplicationIdentifier.AI3354),
								new CodeVerificationTemplateUnit(ApplicationIdentifier.AI3355)
							])
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuNonAlcoholicBeerProducts,
					"Безалкогольная пивная продукция",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 7),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuNonAlcoholicBeerProductsWithVolume,
					"Безалкогольная пивная продукция + объем",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 7),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI3350, default, default,
							[
								new CodeVerificationTemplateUnit(ApplicationIdentifier.AI3351),
								new CodeVerificationTemplateUnit(ApplicationIdentifier.AI3352),
								new CodeVerificationTemplateUnit(ApplicationIdentifier.AI3353),
								new CodeVerificationTemplateUnit(ApplicationIdentifier.AI3354),
								new CodeVerificationTemplateUnit(ApplicationIdentifier.AI3355)
							])
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuPackagedWater,
					"Упакованная вода",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuSoftDrinks,
					"Безалкогольные напитки",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuDairyProducts,
					"Молочная продукция",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 6),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuDairyProductsWithWeight,
					"Молочная продукция + вес",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 6),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI3103)
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuMeatProducts,
					"Мясная продукция",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 6),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI91, 4, "Ключ проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI92, 44, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuSeafood,
					"Морепродукты",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 6),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI91, 4, "Ключ проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI92, 44, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuSeafoodShortened,
					"Морепродукты (укороченный)",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 6),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuCannedProducts,
					"Консервированная продукция",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 6),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI91, 4, "Ключ проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI92, 44, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuCannedProductsShortened,
					"Консервированная продукция (укороченный)",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 6),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuGroceryProducts,
					"Бакалейная продукция",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI91, 4, "Ключ проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI92, 44, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuGroceryProductsShortened,
					"Бакалейная продукция (укороченный)",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuSweetsAndConfectionery,
					"Сладости и кондитерские изделия",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI91, 4, "Ключ проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI92, 44, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuSweetsAndConfectioneryShortened,
					"Сладости и кондитерские изделия (укороченный)",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 6),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuVegetableOils,
					"Растительные масла",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI91, 4, "Ключ проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI92, 44, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuVegetableOilsShortened,
					"Растительные масла (укороченный)",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuDietarySupplement,
					"Биологически активные добавки",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI91, 4, "Ключ проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI92, 44, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuDietarySupplementShortened,
					"Биологически активные добавки (укороченный)",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuPetFood,
					"Корма для животных",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 6),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI91, 4, "Ключ проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI92, 44, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuPetFoodShortened,
					"Корма для животных (укороченный)",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 6),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuLightIndustryProduct,
					"Товар легкой промышленности (белье, предметы одежды)",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI91, 4, "Ключ проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI92, 44, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuFootwear,
					"Обувные товары",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI91, 4, "Ключ проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI92, 88, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuAntisepticsAndDisinfectants,
					"Антисептики и дезинфицирующие средства",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI91, 4, "Ключ проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI92, 44, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuAntisepticsAndDisinfectantsShortened,
					"Антисептики и дезинфицирующие средства (укороченный)",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuMedicines,
					"Лекарственные препараты",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI91, 4, "Ключ проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI92, 44, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuVeterinaryDrugs,
					"Ветеринарные препараты",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI91, 4, "Ключ проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI92, 44, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuMedicalProducts,
					"Медицинские изделия",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI91, 4, "Ключ проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI92, 44, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuPerfume,
					"Духи и туалетная вода",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI91, 4, "Ключ проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI92, 44, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuCosmeticsAndHouseholdChemicals,
					"Косметика и бытовая химия",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 6),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI91, 4, "Ключ проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI92, 44, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuCosmeticsAndHouseholdChemicalsShortened,
					"Косметика и бытовая химия (укороченный)",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 6),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuPrintedProducts,
					"Печатная продукция",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI91, 4, "Ключ проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI92, 44, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuChildrensToys,
					"Детские игрушки",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI91, 4, "Ключ проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI92, 44, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuChildrensToysShortened,
					"Детские игрушки (укороченный)",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 6),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuPyrotechnicsAndFireSafetyEquipment,
					"Пиротехника и средства пожарной безопасности",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI91, 4, "Ключ проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI92, 44, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuPyrotechnicsAndFireSafetyEquipmentShortened,
					"Пиротехника и средства пожарной безопасности (укороченный)",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 6),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuCamerasAndFlashLamps,
					"Фотоаппараты и лампы-вспышки",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 20),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI91, 4, "Ключ проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI92, 44, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuRadioElectronicProducts,
					"Радиоэлектронная продукция",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI91, 4, "Ключ проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI92, 44, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuRadioElectronicProductsShortened,
					"Радиоэлектронная продукция (укороченный)",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuCableAndWireProducts,
					"Кабельно-проводниковая продукция",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI91, 4, "Ключ проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI92, 44, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuCableAndWireProductsShortened,
					"Кабельно-проводниковая продукция (укороченный)",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuFiberOptic,
					"Оптоволокно",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI91, 4, "Ключ проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI92, 44, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuFiberOpticShortened,
					"Оптоволокно (укороченный)",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuTiresAndCovers,
					"Шины и покрышки",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI91, 4, "Ключ проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI92, 44, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuBicycles,
					"Велосипеды",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI91, 4, "Ключ проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI92, 44, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuMotorOils,
					"Моторные масла",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI91, 4, "Ключ проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI92, 44, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuMotorOilsShortened,
					"Моторные масла (укороченный)",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuBuildingMaterials,
					"Строительные материалы",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI91, 4, "Ключ проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI92, 44, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuBuildingMaterialsShortened,
					"Строительные материалы (укороченный)",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuHeatingDevices,
					"Отопительные приборы",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 6),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI91, 4, "Ключ проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI92, 44, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuHeatingDevicesShortened,
					"Отопительные приборы (укороченный)",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 6),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuPolymerPipeProducts,
					"Полимерная трубная продукция",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 6),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI91, 4, "Ключ проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI92, 44, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuPolymerPipeProductsShortened,
					"Полимерная трубная продукция (укороченный)",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 6),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.RuTitaniumMetalProducts,
					"Титановая металлопродукция",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.ByDairyProducts,
					"Молочная продукция (Республика Беларусь)",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 8),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI93, 4, "Код проверки")
					]),

				new CodeVerificationTemplateDescriptor(
					CodeVerificationTemplate.UzMedicines,
					"Лекарственные препараты (Узбекистан)",
					true,
					[
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI01),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI21, 13),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI91, 4, "Ключ проверки"),
						new CodeVerificationTemplateUnit(ApplicationIdentifier.AI92, 44, "Код проверки")
					])
			];
	}
}