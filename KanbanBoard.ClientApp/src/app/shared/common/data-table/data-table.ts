import { Component, contentChildren, EventEmitter, input, Input, Output, output, TemplateRef } from '@angular/core';
import { TableColumn } from './data-table.model';
import { Column } from '../../models/kanban.model';

@Component({
  selector: 'app-data-table',
  imports: [],
  templateUrl: './data-table.html',
})
export class DataTable<T> {

  data = input.required<T[]>();
  columns = input.required<TableColumn<T>[]>();

  rowClick = output<T>();

  customCells = contentChildren<TemplateRef<any>>(TemplateRef);

}
