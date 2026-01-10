import { Component, signal } from '@angular/core';
import { Sidebar } from '../sidebar/sidebar';
import { Header } from '../header/header';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-mail-layout',
  imports: [Sidebar,Header,RouterOutlet],
  templateUrl: './main-layout.html',
  styleUrl: './main-layout.scss',
})
export class MainLayout {

  isSidebarExpanded = signal(true);
    
  toggleSidebar(expanded: boolean) {
    this.isSidebarExpanded.set(expanded);
  }
}
