// ── Types ──────────────────────────────────────────────────────────────────────

export type Difficulty = 'Easy' | 'Medium' | 'Hard';

export interface IngredientDto {
  id: string;
  name: string;
  quantity: number;
  unit: string;
}

export interface RecipeDto {
  id: string;
  name: string;
  description: string;
  instructions: string;
  prepTimeMinutes: number;
  difficulty: Difficulty;
  categoryId: string;
  ingredients: IngredientDto[];
}

export interface CreateIngredientDto {
  name: string;
  quantity: number;
  unit: string;
}

export interface CreateRecipeDto {
  name: string;
  description: string;
  instructions: string;
  prepTimeMinutes: number;
  difficulty: Difficulty;
  categoryId: string;
  ingredients: CreateIngredientDto[];
}

export interface UpdateRecipeDto {
  id: string;
  name: string;
  description: string;
  instructions: string;
  prepTimeMinutes: number;
  difficulty: Difficulty;
}

// ── API calls ─────────────────────────────────────────────────────────────────

const BASE = '/api/recipes';

export async function getRecipes(): Promise<RecipeDto[]> {
  const res = await fetch(BASE);
  if (!res.ok) throw new Error('Failed to fetch recipes');
  return res.json();
}

export async function getRecipeById(id: string): Promise<RecipeDto> {
  const res = await fetch(`${BASE}/${id}`);
  if (!res.ok) throw new Error(`Recipe ${id} not found`);
  return res.json();
}

export async function createRecipe(dto: CreateRecipeDto): Promise<string> {
  const res = await fetch(BASE, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(dto),
  });
  if (!res.ok) throw new Error('Failed to create recipe');
  return res.json();
}

export async function updateRecipe(id: string, dto: UpdateRecipeDto): Promise<void> {
  const res = await fetch(`${BASE}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(dto),
  });
  if (!res.ok) throw new Error('Failed to update recipe');
}

export async function deleteRecipe(id: string): Promise<void> {
  const res = await fetch(`${BASE}/${id}`, { method: 'DELETE' });
  if (!res.ok) throw new Error('Failed to delete recipe');
}
