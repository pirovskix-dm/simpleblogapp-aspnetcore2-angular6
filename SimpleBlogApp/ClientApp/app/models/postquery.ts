export interface PostQuery {
	tagId: number | null;
	categoryId: number | null;
	search: string;
	sortBy: string;
	isSortAscending: boolean;
	page: number;
	pageSize: number;
}