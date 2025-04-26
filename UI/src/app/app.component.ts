import { Component, Host, HostBinding, ViewEncapsulation } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { SpoolsService } from './shared/service/spoolman.service';
import { HttpClientModule } from '@angular/common/http';
import { TrayService } from './shared/service/tray.service';
import { MatSelectChange, MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { AMSEntity, Tray } from './shared/models/tray';
import { Spool } from './shared/models/spool';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    HttpClientModule,
    CommonModule,
    MatToolbarModule,
    RouterModule
  ],
  templateUrl: './app.component.html',
  providers: [SpoolsService, TrayService],
  encapsulation: ViewEncapsulation.None,
})
export class AppComponent {
  @HostBinding('class') className = 'app-root';
  
}
