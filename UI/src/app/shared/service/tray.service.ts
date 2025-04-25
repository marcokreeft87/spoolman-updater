import { HttpClient, HttpParams } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { AMSEntity, Tray } from '../models/tray';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class TrayService {
  private baseUrl = '/'; // change if your API is prefixed

  constructor(private http: HttpClient) {}

  // Get Trays
  getTrays(): Observable<{ ams_entities: AMSEntity[], external_spool_entity: Tray }> {
    return this.http.get<{ ams_entities: AMSEntity[], external_spool_entity: Tray }>(`${this.baseUrl}trays`).pipe(map(response => response));
  }
}
