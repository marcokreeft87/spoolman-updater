import { AsyncPipe, CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { map, Observable, startWith } from 'rxjs';
import { Spool } from '../../models/spool';
import { Tray } from '../../models/tray';
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
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatAutocompleteModule,
    ReactiveFormsModule,
    AsyncPipe,
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
  filteredSpools: Observable<Spool[]> = new Observable<Spool[]>();
   spoolControl = new FormControl('');

  constructor(private spoolService: SpoolsService) { }

  ngOnInit(): void {
    this.currentSpool = this.getCurrentSpool(this.tray);

    this.filteredSpools = this.spoolControl.valueChanges.pipe(
      startWith(''),
      map(value => this.filter(value || '')),
    );
  }

  filter(value: string | Spool): Spool[] {
    const filterValue = typeof value !== 'string' ? this.displaySpoolName(value as Spool) : value.toString().toLowerCase();

    return this.spools.filter(option => this.displaySpoolName(option).toLowerCase().includes(filterValue));
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

  onSpoolChange(selectChange: MatAutocompleteSelectedEvent, tray: Tray): void {
    const selectedSpoolId = selectChange.option.value.id;

    console.log(selectedSpoolId, tray);
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
        this.spoolControl.setValue(null);
      });
  }
}
