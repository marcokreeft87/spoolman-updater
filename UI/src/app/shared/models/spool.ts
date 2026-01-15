import { Filament } from "./filament";

export interface Spool {
    id: number;
    registered: string;
    first_used: string;
    last_used: string;
    filament_id: number;
    filament: Filament;
    price: number | null;
    remaining_weight: number;
    initial_weight: number;
    spool_weight: number;
    used_weight: number;
    remaining_length: number;
    used_length: number;
    location: string | null;
    lot_number: string | null;
    comment: string | null;
    archived: boolean;
    extra: Record<string, string>;
    barcode?: string | null;
  }
  
  
  
