import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectChange, MatSelectModule } from '@angular/material/select';
import { Tray } from '../../models/tray';
import { Spool } from '../../models/spool';
import { SpoolsService } from '../../service/spoolman.service';
import { SpoolItemComponent } from "../spool/spool.component";

@Component({
  selector: 'app-tray',
  standalone: true,
  imports: [
    MatButtonModule,
    MatCardModule,
    MatSelectModule,
    MatFormFieldModule,
    CommonModule,
    SpoolItemComponent
],
  templateUrl: './tray.component.html',
  styleUrls: ['./tray.component.scss'],
})
export class TrayComponent implements OnInit {
  @Input() tray: Tray | null = null;
  @Input() spools: Spool[] = [];
  @Input() name: string = '';

  currentSpool: Spool | undefined;

  constructor(private spoolService: SpoolsService) { }

  ngOnInit(): void {
    this.currentSpool = this.getCurrentSpool(this.tray);
  }

  displaySpoolName(spool: Spool): string {
    return spool
      ? `${spool.filament.vendor.name} ${spool.filament.material} ${spool.filament.name} - ${spool.remaining_weight}g`
      : '';
  }

  getCurrentSpool(tray: Tray | null): Spool {
    if (!tray) {
      return {} as Spool;
    }

    const currentSpool = this.spools.filter((spool) =>
      spool.extra['active_tray']?.includes(tray.id)
    )[0];

    return currentSpool;
  }

  onSpoolChange(selectChange: MatSelectChange, tray: Tray): void {
    const selectedSpoolId = selectChange.value;

    this.setSpoolToTray(selectedSpoolId, tray);
  }

  setSpoolToTray(selectedSpoolId: string, tray: Tray): void {
    tray.selectedSpool = selectedSpoolId;    
  }

  updateTray(tray: Tray | undefined): void {
    if (!tray) return;

    this.spoolService
      .updateTray({
        spool_id: tray.selectedSpool,
        active_tray_id: tray.id,
      })
      .subscribe(spool => {
        console.log('Tray updated successfully!');

        this.currentSpool = spool;
        console.log('Current spool:', spool);
      });
  }
}
