export interface Tray {
  id: string;
  name: string;
  color: string;
  tag_uid: string;
  type: string;
  selectedSpool: string;
}

export interface AMSEntity {
  id: string;
  trays: Tray[];
}