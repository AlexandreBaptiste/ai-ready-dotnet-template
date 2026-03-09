import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import type { CreateIngredientDto, Difficulty } from '../../api/recipesApi';
import { createRecipe, getRecipeById, updateRecipe } from '../../api/recipesApi';
import type { CategoryDto } from '../../api/categoriesApi';
import { getCategories } from '../../api/categoriesApi';

interface FormState {
  name: string;
  description: string;
  instructions: string;
  prepTimeMinutes: number;
  difficulty: Difficulty;
  categoryId: string;
  ingredients: CreateIngredientDto[];
}

const EMPTY: FormState = {
  name: '',
  description: '',
  instructions: '',
  prepTimeMinutes: 30,
  difficulty: 'Easy',
  categoryId: '',
  ingredients: [],
};

export default function RecipeFormPage() {
  const { id } = useParams<{ id: string }>();
  const isEdit = Boolean(id);
  const navigate = useNavigate();

  const [form, setForm] = useState<FormState>(EMPTY);
  const [categories, setCategories] = useState<CategoryDto[]>([]);
  const [errors, setErrors] = useState<Partial<Record<keyof FormState, string>>>({});
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    getCategories().then(setCategories).catch(console.error);
    if (isEdit && id) {
      getRecipeById(id)
        .then((r) =>
          setForm({
            name: r.name,
            description: r.description,
            instructions: r.instructions,
            prepTimeMinutes: r.prepTimeMinutes,
            difficulty: r.difficulty,
            categoryId: r.categoryId,
            ingredients: r.ingredients.map((i) => ({
              name: i.name,
              quantity: i.quantity,
              unit: i.unit,
            })),
          })
        )
        .catch(console.error);
    }
  }, [id, isEdit]);

  const set = <K extends keyof FormState>(key: K, value: FormState[K]) =>
    setForm((prev) => ({ ...prev, [key]: value }));

  const validate = (): boolean => {
    const e: Partial<Record<keyof FormState, string>> = {};
    if (!form.name.trim()) e.name = 'Name is required.';
    if (!form.description.trim()) e.description = 'Description is required.';
    if (!form.instructions.trim()) e.instructions = 'Instructions are required.';
    if (form.prepTimeMinutes <= 0) e.prepTimeMinutes = 'Prep time must be > 0.';
    if (!form.categoryId) e.categoryId = 'Please select a category.';
    setErrors(e);
    return Object.keys(e).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!validate()) return;
    setSaving(true);
    try {
      if (isEdit && id) {
        await updateRecipe(id, { ...form, id });
      } else {
        await createRecipe(form);
      }
      navigate('/recipes');
    } catch (err: unknown) {
      alert((err as Error).message);
    } finally {
      setSaving(false);
    }
  };

  const addIngredient = () =>
    set('ingredients', [...form.ingredients, { name: '', quantity: 1, unit: 'g' }]);

  const updateIngredient = (i: number, patch: Partial<CreateIngredientDto>) =>
    set(
      'ingredients',
      form.ingredients.map((ing, idx) => (idx === i ? { ...ing, ...patch } : ing))
    );

  const removeIngredient = (i: number) =>
    set('ingredients', form.ingredients.filter((_, idx) => idx !== i));

  const field = (
    label: string,
    key: keyof FormState,
    input: React.ReactNode
  ) => (
    <div>
      <label className="block text-sm font-medium text-gray-700 mb-1">{label}</label>
      {input}
      {errors[key] && <p className="text-red-600 text-xs mt-1">{errors[key]}</p>}
    </div>
  );

  const inputCls =
    'w-full border border-gray-300 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-400';

  return (
    <div className="p-6 max-w-2xl mx-auto">
      <h1 className="text-2xl font-bold text-gray-800 mb-6">
        {isEdit ? 'Edit Recipe' : 'New Recipe'}
      </h1>

      <form onSubmit={handleSubmit} className="space-y-5 bg-white rounded-xl shadow p-6">
        {field(
          'Name',
          'name',
          <input
            className={inputCls}
            value={form.name}
            onChange={(e) => set('name', e.target.value)}
          />
        )}
        {field(
          'Description',
          'description',
          <textarea
            className={inputCls}
            rows={3}
            value={form.description}
            onChange={(e) => set('description', e.target.value)}
          />
        )}
        {field(
          'Instructions',
          'instructions',
          <textarea
            className={inputCls}
            rows={5}
            value={form.instructions}
            onChange={(e) => set('instructions', e.target.value)}
          />
        )}

        <div className="grid grid-cols-2 gap-4">
          {field(
            'Prep Time (min)',
            'prepTimeMinutes',
            <input
              type="number"
              className={inputCls}
              min={1}
              value={form.prepTimeMinutes}
              onChange={(e) => set('prepTimeMinutes', parseInt(e.target.value, 10))}
            />
          )}
          {field(
            'Difficulty',
            'difficulty',
            <select
              className={inputCls}
              value={form.difficulty}
              onChange={(e) => set('difficulty', e.target.value as Difficulty)}
            >
              <option value="Easy">Easy</option>
              <option value="Medium">Medium</option>
              <option value="Hard">Hard</option>
            </select>
          )}
        </div>

        {field(
          'Category',
          'categoryId',
          <select
            className={inputCls}
            value={form.categoryId}
            onChange={(e) => set('categoryId', e.target.value)}
          >
            <option value="">— select —</option>
            {categories.map((c) => (
              <option key={c.id} value={c.id}>
                {c.name}
              </option>
            ))}
          </select>
        )}

        {/* Ingredients */}
        <div>
          <div className="flex items-center justify-between mb-2">
            <label className="text-sm font-medium text-gray-700">Ingredients</label>
            <button
              type="button"
              onClick={addIngredient}
              className="text-indigo-600 text-sm hover:underline"
            >
              + Add
            </button>
          </div>
          {form.ingredients.map((ing, i) => (
            <div key={i} className="flex gap-2 mb-2 items-center">
              <input
                placeholder="Name"
                className={`${inputCls} flex-1`}
                value={ing.name}
                onChange={(e) => updateIngredient(i, { name: e.target.value })}
              />
              <input
                type="number"
                placeholder="Qty"
                className={`${inputCls} w-24`}
                min={0.01}
                step={0.01}
                value={ing.quantity}
                onChange={(e) => updateIngredient(i, { quantity: parseFloat(e.target.value) })}
              />
              <input
                placeholder="Unit"
                className={`${inputCls} w-20`}
                value={ing.unit}
                onChange={(e) => updateIngredient(i, { unit: e.target.value })}
              />
              <button
                type="button"
                onClick={() => removeIngredient(i)}
                className="text-red-500 hover:text-red-700 text-lg leading-none"
              >
                ×
              </button>
            </div>
          ))}
        </div>

        <div className="flex gap-3 pt-2">
          <button
            type="submit"
            disabled={saving}
            className="bg-indigo-600 hover:bg-indigo-700 disabled:opacity-50 text-white px-5 py-2 rounded-lg text-sm font-medium"
          >
            {saving ? 'Saving…' : isEdit ? 'Save Changes' : 'Create Recipe'}
          </button>
          <button
            type="button"
            onClick={() => navigate('/recipes')}
            className="text-gray-600 hover:text-gray-800 px-4 py-2 text-sm"
          >
            Cancel
          </button>
        </div>
      </form>
    </div>
  );
}
