/**
 * Used to set game objects as interactable when interacted with by the player.
 */
interface Interactable
{
    /**
     * Will be called by the player when they press the interact button with their interact range
     * 
     * Player (player): the player interacting with the game object.
     */
    void Interact(Player player);
}
