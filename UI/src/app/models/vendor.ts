export interface Vendor {
  id: number;
  registered: string;
  name: string;
  comment: string | null;
  empty_spool_weight: number | null;
  external_id: string;
  extra: Record<string, unknown>;
}
