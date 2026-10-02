// Portfolios

export type PortfolioType = {
  id: string;
  name?: string;
  description?: string;
  imageUrl?: string;
  totalValue?: number | null;
  unrealizedGain?: number | null;
  createdAt?: Date;
  holdings?: number;
  assets?: PortfolioAssetType[];
};

export type EtfThemeType = {
  id: number;
  name: string;
  description?: string;
  imageUrl?: string;
  theme: string;
};

export type EtfByIdItemType = {
  isin: number;
  name: string;
  fundSize: number;
  ter: number;
  useOfProfits: string;
  fundDomicile: string;
  replicationMethod: string;
};

export type EtfThemeByIdType = {
  name?: string;
  imageUrl?: string;
  description?: string;
  items: EtfByIdItemType[];
};

export type IndiceSummaryType = {
  id: string;
  name?: string;
  description?: string;
  imageUrl?: string;
  totalValue?: number | null;
  unrealizedGain?: number | null;
  createdAt?: Date;
  holdings?: number;
  assets?: PortfolioAssetType[];
};

export type PortfoliosAreaType = {
  portfolios: string[];
  performance: PortfolioAreaDto[];
};

export type IndicesAreaType = {
  indices: string[];
  performance: IndiceAreaDto[];
};

export type IndiceAreaDto = {
  month: string;
  indices: {
    name: string;
    performance: number;
  }[];
};

export type PortfolioAreaDto = {
  month: string;
  portfolios: {
    name: string;
    performance: string;
  }[];
};

export type PortfolioAssetType = {
  symbol: string;
  numberOfShares: number;
  pricePerShare: number;
};

// Watchlists

export type WatchlistItemType = {
  id: string;
  name: string;
  description: string;
  ticker: string;
};

export type WatchListType = {
  id: string;
  name: string;
  items: WatchlistItemType[];
};

export type WatchListPageType = {
  items: WatchListType[];
  nextCursor: string | null;
};

// Others

export type TopNewsType = {
  id: number;
  text: string;
  date?: number;
  sentiment?: number;
  country?: string;
  source?: string;
  description?: string;
  url?: string;
};

export type TopNewsDataType = {
  items: TopNewsType[];
  date?: Date;
};

export type PortfolioPerformanceType = {
  id: string;
  portfolioName: string;
  "7d": number;
  "1y": number;
};

export type IndicePerformanceType = {
  id: number;
  name: string;
  countryCode: string;
  regularMarketChangePercent: number;
  regularMarketPrice: number;
};

export type IndicePerformanceDataType = {
  items: IndicePerformanceType[];
  date?: Date;
};

export enum SectorType {
  BasicMaterials = "Basic Materials",
  Telecom = "Telecom",
  ConsumerGoods = "Consumer Goods",
  CustomerStaples = "Customer Staples",
  ConsumerServices = "Consumer Services",
  Energy = "Energy",
  Financials = "Financials",
  HealthCare = "Health Care",
  Industrials = "Industrials",
  Materials = "Materials",
  Utilities = "Utilities",
  Technology = "Technology",
}

export type SectorPerformanceType = {
  type: SectorType;
  change: number;
  country: string;
};

export type SectorPerformanceDataType = {
  items: SectorPerformanceType[];
  date?: Date;
};

export enum CountryType {
  US = "United States",
  CA = "Canada",
  GB = "United Kingdom",
  DE = "Germany",
  FR = "France",
  AU = "Australia",
}

export type EquityType = {
  country: CountryType;
  change: number;
  lastPrice: number;
  volume: number;
};

type ItemType = "s" | "c" | "f";

export type LazyPortfolioItemType = {
  id: number;
  weight: number;
  ticker: string;
  name: string;
  themes: string;
  type: ItemType;
};

export type LazyPortfolioType = {
  id: number;
  name: string;
  description?: string;
  "1d": number;
  ytd: number;
  dividendYield: number;
  performanceRank: number;
  maxDrawdown: number;
  expenseRatio: number;
  sharpeRatio: number;
  items: LazyPortfolioItemType[];
};

export type TreeMapType = {
  sector: SectorType;
  companies: {
    company: string;
    change: number;
  }[];
};

export type GainersType = {
  type: SectorType;
  change: number;
  country: string;
};

export type GainersDataType = {
  items: GainersType[];
  date?: Date;
};

export type LosersType = {
  type: SectorType;
  change: number;
  country: string;
};

export type LosersDataType = {
  items: LosersType[];
  date?: Date;
};

export type TopIndustriesType = {
  type: SectorType;
  change: number;
  country: string;
};

export type TopIndustriesDataType = {
  items: TopIndustriesType[];
  date?: Date;
};

export type WorstIndustriesType = {
  type: SectorType;
  change: number;
  country: string;
};

export type WorstIndustriesDataType = {
  items: WorstIndustriesType[];
  date?: Date;
};
