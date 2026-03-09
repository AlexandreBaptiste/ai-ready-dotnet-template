// ── Types ──────────────────────────────────────────────────────────────────────

export interface CategoryDto {
  id: string;
  name: string;
}

export interface CreateCategoryDto {
  name: string;
}

export interface UpdateCategoryDto {
  id: string;
  name: string;
}

// ── API calls ─────────────────────────────────────────────────────────────────

const BASE = '/api/categories';

export async function getCategories(): Promise<CategoryDto[]> {
  const res = await fetch(BASE);
  if (!res.ok) throw new Error('Failed to fetch categories');
  return res.json();
}

export async function createCategory(dto: CreateCategoryDto): Promise<string> {
  const res = await fetch(BASE, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(dto),
  });
  if (!res.ok) throw new Error('Failed to create category');
  const body = await res.json();
  return body.id ?? body;
}

export async function updateCategory(id: string, dto: UpdateCategoryDto): Promise<void> {
  const res = await fetch(`${BASE}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(dto),
  });
  if (!res.ok) throw new Error('Failed to update category');
}

export async function deleteCategory(id: string): Promise<void> {
  const res = await fetch(`${BASE}/${id}`, { method: 'DELETE' });
  if (!res.ok) throw new Error('Failed to delete category');
}
