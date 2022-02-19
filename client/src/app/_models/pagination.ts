export interface Pagination {
    currentPage: number;
    totalItems: number;
    totalPages: number;
    itemsPerPage: number;
}

export class PaginatedResult<T>{
    result: T;
    Pagination: Pagination
}