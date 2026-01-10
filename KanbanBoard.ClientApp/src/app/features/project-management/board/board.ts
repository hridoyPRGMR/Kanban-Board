import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Column, INITIAL_COLUMNS } from '../../../shared/models/kanban.model';
import { KanabanColumn } from '../../../shared/common/kanban-column/kanban-column';

@Component({
  selector: 'app-board',
  imports: [CommonModule, KanabanColumn],
  templateUrl: './board.html',
  styleUrl: './board.scss',
})
export class Board {
  columns = signal<Column[]>(INITIAL_COLUMNS);
}