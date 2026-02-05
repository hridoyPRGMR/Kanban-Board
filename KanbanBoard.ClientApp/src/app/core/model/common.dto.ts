export interface PaginatedResponse<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

export interface PagedAndSortedResultRequestDto{
  pageNumber: number;
  pageSize:number;
  sortedBy?:string;
  isDescending: boolean;
  searchTerm?: string;
}