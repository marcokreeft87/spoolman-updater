import { Component } from '@angular/core';
import { SpoolsService } from '../../shared/service/spoolman.service';
import { TrayService } from '../../shared/service/tray.service';
import { Spool } from '../../shared/models/spool';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { AMSEntity, Tray } from '../../shared/models/tray';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { SpoolItemComponent } from '../../shared/components/spool/spool.component';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-scan',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule, SpoolItemComponent, RouterModule],
  templateUrl: './scan.component.html',
  styleUrl: './scan.component.scss',
  providers: [SpoolsService, TrayService],
})
export class ScanComponent {
  spool: Spool | undefined;
  amsEntities: AMSEntity[] = [];
  externalSpoolEntity: Tray = {} as Tray;
  loading = true;
  error: string | null = null;
  updating = false;

  constructor(
    private route: ActivatedRoute,
    private spoolService: SpoolsService,
    private router: Router,
    private trayService: TrayService
  ) {

    this.route.queryParamMap.subscribe(params => {
      const barcode = params.get('barcode') ?? '';
      this.loading = true;
      this.error = null;

      if (!barcode) {
        this.loading = false;
        this.error = 'No barcode provided';
        return;
      }

      // Try to parse as Spoolman QR code format first
      const spoolId = this.spoolService.parseSpoolmanQrCode(barcode);

      if (spoolId) {
        // Look up by spool ID
        this.loadSpoolById(spoolId);
      } else {
        // Fall back to barcode lookup
        this.loadSpoolByBarcode(barcode);
      }
    });
  }

  private loadSpoolById(spoolId: string): void {
    forkJoin({
      spools: this.spoolService.getSpools(),
      trays: this.trayService.getTrays()
    }).subscribe({
      next: ({ spools, trays }) => {
        this.loading = false;
        const spool = spools.find(s => s.id.toString() === spoolId);
        if (!spool) {
          this.error = `No spool found with ID: ${spoolId}`;
          return;
        }
        this.spool = spool;
        this.amsEntities = trays.ams_entities;
        this.externalSpoolEntity = trays.external_spool_entity;
      },
      error: (err) => {
        this.loading = false;
        this.error = `Failed to load spool: ${err.message || 'Unknown error'}`;
      }
    });
  }

  private loadSpoolByBarcode(barcode: string): void {
    forkJoin({
      spools: this.spoolService.getSpools(),
      trays: this.trayService.getTrays()
    }).subscribe({
      next: ({ spools, trays }) => {
        this.loading = false;
        const spool = spools.find(s => s.extra?.['barcode'] === JSON.stringify(barcode));
        if (!spool) {
          this.error = `No spool found for barcode: ${barcode}`;
          return;
        }
        this.spool = spool;
        this.amsEntities = trays.ams_entities;
        this.externalSpoolEntity = trays.external_spool_entity;
      },
      error: (err) => {
        this.loading = false;
        this.error = `Failed to load spool: ${err.message || 'Unknown error'}`;
      }
    });
  }

  updateTray(tray: Tray | undefined): void {
    if (!tray || !this.spool || this.updating)
      return;

    this.updating = true;
    this.error = null;

    this.spoolService
      .updateTray({
        spool_id: this.spool.id.toString(),
        active_tray_id: tray.id,
      })
      .subscribe({
        next: () => {
          this.updating = false;
          this.router.navigate(['/']);
        },
        error: (err) => {
          this.updating = false;
          this.error = `Failed to update tray: ${err.message || 'Unknown error'}`;
        }
      });
  }
}
