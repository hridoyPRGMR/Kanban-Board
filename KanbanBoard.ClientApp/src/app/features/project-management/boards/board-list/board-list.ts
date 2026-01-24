import { Component, computed, inject, signal } from '@angular/core';
import { BoardService } from '@core/api/board.service';
import { BoardDto } from '@core/model/project-management.dto';
import { DataTable } from 'src/app/shared/common/data-table/data-table';
import { TableColumn } from 'src/app/shared/common/data-table/data-table.model';

@Component({
  selector: 'app-board-list',
  imports: [DataTable],
  templateUrl: './board-list.html',
})
export class BoardList {
  
  private readonly boardService = inject(BoardService);
  searchTerm = signal('');

  boardsResource = this.boardService.getBoards(()=> ({
    search: this.searchTerm()
  }));

  boardData = computed(() => this.boardsResource.value() ?? []);

  columns: TableColumn<BoardDto>[] = [
    { key: 'id', header: 'Id' },
    { key: 'name', header: 'Full Name', sortable: true }
  ];

  handleRowClick(board: BoardDto) {
    console.log('View Board: ', board.name);
  }
}
