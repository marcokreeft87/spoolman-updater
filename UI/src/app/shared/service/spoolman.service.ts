import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { UpdateTrayInput } from '../models/updatetray';
import { Spool } from '../models/spool';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class SpoolsService {
  private baseUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) {}

  /**
   * GET /Spools
   */
  getSpools(): Observable<Spool[]> {
    return this.http.get<any>(`${this.baseUrl}spools`).pipe(map(response => response.spools));
  }

  /**
   * POST /Spools/tray
   */
  updateTray(data: UpdateTrayInput): Observable<Spool> {
    return this.http.post<any>(`${this.baseUrl}spools/tray`, data).pipe(map(response => response.spool));
  }

  /**
   * Parse Spoolman QR code formats to extract spool ID
   * Supported formats:
   * - web+spoolman:s-{id} (e.g., "web+spoolman:s-4")
   * - https://.../spool/show/{id} (e.g., "https://spoolman.example.com:7913/spool/show/4")
   * - Plain number (e.g., "4")
   */
  parseSpoolmanQrCode(value: string): string | null {
    if (!value) return null;

    // Format: web+spoolman:s-{id}
    const webSpoolmanMatch = value.match(/^web\+spoolman:s-(\d+)$/i);
    if (webSpoolmanMatch) {
      return webSpoolmanMatch[1];
    }

    // Format: URL ending in /spool/show/{id} or /spool/{id}
    const urlMatch = value.match(/\/spool(?:\/show)?\/(\d+)\/?$/i);
    if (urlMatch) {
      return urlMatch[1];
    }

    // Plain number
    if (/^\d+$/.test(value.trim())) {
      return value.trim();
    }

    return null;
  }
}
