import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { Configuration } from '../models/configuration';

@Injectable({
  providedIn: 'root',
})
export class SettingssService {
  private baseUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) {}

  /**
   * GET /Spools
   */
  get(): Observable<Configuration> {
    return this.http.get<any>(`${this.baseUrl}settings`).pipe(map(response => response.configuration));
  }

  /**
   * POST /Spools/tray
   */
  // updateTray(data: UpdateTrayInput): Observable<Spool> {
  //   return this.http.post<any>(`${this.baseUrl}spools/tray`, data).pipe(map(response => response.spool));
  // }
}
