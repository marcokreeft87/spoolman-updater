import { Vendor } from "./vendor";

export interface Filament {
    id: number;
    registered: string;
    name: string;
    vendor_id: number;
    vendor: Vendor;
    material: string;
    price: number;
    density: number;
    diameter: number;
    weight: number;
    spool_weight: number;
    article_number: string | null;
    comment: string | null;
    extruder_temp: number;
    bed_temp: number;
    color_hex: string;
    multi_color_hexes: string | null;
    multi_color_direction: string | null;
    external_id: string | null;
    extra: Record<string, unknown>;
  }