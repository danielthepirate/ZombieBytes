using UnityEngine;

public class LootTable : MonoBehaviour {

	public Loot[] table;

	public void DropLoot() {
		Loot loot = ChooseLoot();

		if (loot == null) { return; }

		Vector3 lootPosition = new Vector3(transform.position.x, 0.8f, transform.position.z);
		GameObject newLoot = Instantiate(loot.item);
		newLoot.transform.position = lootPosition;
	}

	private Loot ChooseLoot() {
		int roll = Random.Range(1, 100);
		int lootChance = 0;

		foreach (Loot loot in table) {
			lootChance += loot.dropChance;
			if (roll <= lootChance) {
				return loot;
			}
		}
		return null;
	}
}
