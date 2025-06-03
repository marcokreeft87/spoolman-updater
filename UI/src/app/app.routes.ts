import { Routes } from '@angular/router';
import { ScanComponent } from './components/scan/scan.component';
import { SpoolComponent } from './components/spool/spool.component';

export const routes: Routes = [
    { path: '', component: SpoolComponent },
    { path: 'scan', component: ScanComponent }
];
