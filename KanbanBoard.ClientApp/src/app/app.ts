import { CommonModule } from '@angular/common';
import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Sidebar } from './components/layout/sidebar/sidebar';
import { Header } from './components/layout/header/header';
import { KanabanColumn } from './components/kanban-column/kanban-column';
import { Column, INITIAL_COLUMNS } from './models/kanban.model';

@Component({
  selector: 'app-root',
  imports: [CommonModule, Sidebar, Header, KanabanColumn],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('KanbanBoard.ClientApp');

  isSidebarExpanded = signal(true);
  columns = signal<Column[]>(INITIAL_COLUMNS);
  
  constructor() {
    console.log('Kanban Board App Initialized with Signals and Standalone Components.');
  }
}
